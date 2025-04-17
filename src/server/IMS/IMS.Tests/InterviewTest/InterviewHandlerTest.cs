using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.Mappings;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Core.ViewModels;
using IMS.Data;
using IMS.Data.UnitOfWorks;
using IMS.Domain;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace IMS.Tests.InterviewTest;

public class InterviewHandlerTest
{
    private ApplicationDbContext _context;
    private Mock<UserManager<User>> _userManager;
    private Mock<RoleManager<Role>> _roleManager;
    private IUnitOfWorks _unitOfWorks;
    private Mock<IEmailService> _emailService;
    private Mock<IHangfireBackgroundService> _backgroundService;
    private IMapper _mapper;
    private IHttpContextAccessor _httpContextAccessor;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"ApplicationDbTest_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);

        _userManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null!, null!, null!, null!, null!, null!, null!, null!
        );

        _userManager.Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Callback((User user) => _context.Users.Add(user))
            .ReturnsAsync(IdentityResult.Success);

        _roleManager = new Mock<RoleManager<Role>>(
            new Mock<IRoleStore<Role>>().Object,
            null!, null!, null!, null!
        );

        _roleManager.Setup(x => x.CreateAsync(It.IsAny<Role>()))
            .Callback((Role role) => _context.Roles.Add(role))
            .ReturnsAsync(IdentityResult.Success);

        await SeedData.SeedDepartments(_context);
        await SeedData.SeedRoles(_context, _roleManager.Object);
        await SeedData.SeedUsers(_context, _userManager.Object);
        await SeedData.SeedCandidates(_context);
        await SeedData.SeedJobs(_context);
        await SeedData.SeedInterviews(_context);

        _unitOfWorks = new UnitOfWorks(_context);
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "nguyenvana@gmail.com"),
            new Claim(ClaimTypes.Role, "ADMIN")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);

        _httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _emailService = new Mock<IEmailService>();

        _emailService.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _backgroundService = new Mock<IHangfireBackgroundService>();

        _backgroundService.Setup(e => e.ScheduleBackgroundJob(It.IsAny<Expression<Func<Task>>>(), It.IsAny<DateTime>()))
            .Returns("mock-background-id");

        _backgroundService.Setup(e => e.DeleteBackgroundJob(It.IsAny<string>()))
            .Returns(true);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.Database.EnsureDeletedAsync();
        _unitOfWorks.Dispose();
        await _context.DisposeAsync();
    }

    [Test]
    public async Task InterviewGetAllQuery_ReturnsAllInterviews()
    {
        var expectedInterviewCount = await _context.Interviews.CountAsync();
        var query = new InterviewGetAllQuery();
        var handler = new InterviewGetAllQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(query, CancellationToken.None);
        var interviews = result.ToList();

        Assert.That(interviews, Is.Not.Null);
        Assert.That(interviews, Is.InstanceOf<List<InterviewViewModel>>());
        Assert.That(interviews.Count, Is.EqualTo(expectedInterviewCount));
        Assert.That(interviews.Any(i => i.Title == "Java Developer Interview"), Is.True);
    }

    [Test]
    public async Task InterviewGetByIdQuery_WithValidId_ReturnsInterview()
    {
        var existingInterview = await _context.Interviews.FirstAsync();
        var query = new InterviewGetByIdQuery { Id = existingInterview.Id };
        var handler = new InterviewGetByIdQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<InterviewViewModel>());
        Assert.That(result.Id, Is.EqualTo(existingInterview.Id));
        Assert.That(result.Title, Is.EqualTo(existingInterview.Title));
    }

    [Test]
    public void InterviewGetByIdQuery_WithInvalidId_ThrowsResourceNotFoundException()
    {
        var query = new InterviewGetByIdQuery { Id = 999 }; // Non-existent ID
        var handler = new InterviewGetByIdQueryHandler(_unitOfWorks, _mapper);

        Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            handler.Handle(query, CancellationToken.None));
    }

    [Test]
    public async Task InterviewCreateCommand_WithValidData_ReturnsNewInterview()
    {
        var initialCount = await _context.Interviews.CountAsync();
        var command = new InterviewCreateUpdateCommand
        {
            Title = "New Interview",
            InterviewDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = TimeOnly.FromDateTime(DateTime.UtcNow),
            EndTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(1),
            CandidateId = 1,
            JobId = 1,
            InterviewersId = [2],
            RecruiterId = 2,
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        var result = await handler.Handle(command, CancellationToken.None);
        var newCount = await _context.Interviews.CountAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo(command.Title));
        Assert.That(newCount, Is.EqualTo(initialCount + 1));
    }

    [Test]
    public void InterviewCreateCommand_WithInvalidCandidate_ThrowsException()
    {
        var command = new InterviewCreateUpdateCommand
        {
            Title = "New Interview",
            InterviewDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = TimeOnly.FromDateTime(DateTime.UtcNow),
            EndTime = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(1),
            CandidateId = 999,
            JobId = 1,
            InterviewersId = [2],
            RecruiterId = 2,
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        Assert.ThrowsAsync<NullReferenceException>(() =>
            handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task InterviewUpdateCommand_WithValidData_ReturnsUpdatedInterview()
    {
        var existedInterview = await _context.Interviews.FirstAsync();
        var command = new InterviewCreateUpdateCommand
        {
            Id = existedInterview.Id,
            Title = "Updated Interview",
            InterviewDate = existedInterview.InterviewDate,
            StartTime = existedInterview.StartTime,
            EndTime = existedInterview.EndTime,
            CandidateId = existedInterview.CandidateId!.Value,
            JobId = existedInterview.JobId!.Value,
            InterviewersId = [.. existedInterview.Interviewers!.Select(i => i.Id)],
            RecruiterId = existedInterview.RecruiterId!.Value,
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo(command.Title));
    }

    [Test]
    public async Task InterviewUpdateCommand_WithInvalidId_ThrowsException()
    {
        var existedInterview = await _context.Interviews.FirstAsync();
        var command = new InterviewCreateUpdateCommand
        {
            Id = 999,
            Title = "Updated Title",
            InterviewDate = existedInterview.InterviewDate,
            StartTime = existedInterview.StartTime,
            EndTime = existedInterview.EndTime,
            CandidateId = existedInterview.CandidateId!.Value,
            JobId = existedInterview.JobId!.Value,
            InterviewersId = [.. existedInterview.Interviewers!.Select(i => i.Id)],
            RecruiterId = existedInterview.RecruiterId!.Value,
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        Assert.ThrowsAsync<ResourceNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task InterviewUpdateCommand_WithInterviewResult_ReturnsUpdatedInterview()
    {
        var existedInterview = await _context.Interviews.FirstAsync();
        var command = new InterviewCreateUpdateCommand
        {
            Id = existedInterview.Id,
            Title = existedInterview.Title,
            InterviewDate = existedInterview.InterviewDate,
            StartTime = existedInterview.StartTime,
            EndTime = existedInterview.EndTime,
            CandidateId = existedInterview.CandidateId!.Value,
            JobId = existedInterview.JobId!.Value,
            InterviewersId = [.. existedInterview.Interviewers!.Select(i => i.Id)],
            RecruiterId = existedInterview.RecruiterId!.Value,
            Result = InterviewResult.Passed.ToString(),
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        var candidate = await _context.Candidates.FirstAsync(c => c.Id == existedInterview.CandidateId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Result, Is.EqualTo(command.Result));
        Assert.That(candidate.Status, Is.EqualTo("Passed Interview"));
    }

    [Test]
    public async Task InterviewUpdateCommand_WithCancelledStatus_ReturnsUpdatedInterview()
    {
        var existedInterview = await _context.Interviews.FirstAsync();
        var command = new InterviewCreateUpdateCommand
        {
            Id = existedInterview.Id,
            Title = existedInterview.Title,
            InterviewDate = existedInterview.InterviewDate,
            StartTime = existedInterview.StartTime,
            EndTime = existedInterview.EndTime,
            CandidateId = existedInterview.CandidateId!.Value,
            JobId = existedInterview.JobId!.Value,
            InterviewersId = [.. existedInterview.Interviewers!.Select(i => i.Id)],
            RecruiterId = existedInterview.RecruiterId!.Value,
            Status = InterviewStatus.Cancelled.ToString(),
        };

        var handler = new InterviewCreateUpdateCommandHandler
            (_unitOfWorks, _mapper, _httpContextAccessor, _backgroundService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(command.Status));
    }

    [Test]
    public async Task InterviewSearchQuery_WithInterviewer_ReturnsMatchedInterviews()
    {
        var interviewer = await _context.Users.FirstAsync();
        var command = new InterviewSearchQuery
        {
            InterviewerId = interviewer.Id,
        };

        var handler = new InterviewSearchQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PaginatedResult<InterviewViewModel>>());
        Assert.That(result.Items, Is.Not.Empty);
        Assert.That(result.Items.All(i => i.InterviewersId!.Contains(command.InterviewerId.Value)), Is.True);
    }

    [Test]
    public async Task InterviewSearchQuery_WithMatchedKeyword_ReturnsMatchedInterviews()
    {
        var command = new InterviewSearchQuery
        {
            Keyword = "Java",
            PageNumber = 1,
            PageSize = 5,
        };

        var handler = new InterviewSearchQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PaginatedResult<InterviewViewModel>>());
        Assert.That(result.Items, Is.Not.Empty);
        Assert.That(result.Items.All(i => i.Title.Contains(command.Keyword)), Is.True);
    }

    [Test]
    public async Task InterviewSearchQuery_WithPageSettings_ReturnsMatchedInterviews()
    {
        var command = new InterviewSearchQuery
        {
            PageNumber = 1,
            PageSize = 5,
        };

        var handler = new InterviewSearchQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PaginatedResult<InterviewViewModel>>());
        Assert.That(result.PageNumber, Is.EqualTo(command.PageNumber));
        Assert.That(result.PageSize, Is.EqualTo(command.PageSize));
        Assert.That(result.TotalCount, Is.EqualTo(result.Items.Length));
        Assert.That(result.TotalPages, Is.EqualTo(Math.Ceiling((float) result.TotalCount / result.PageSize)));
    }

    [Test]
    public async Task InterviewSearchQuery_WithNoMatchedKeyword_ReturnsMatchedInterviews()
    {
        var command = new InterviewSearchQuery
        {
            Keyword = "No Matched",
        };

        var handler = new InterviewSearchQueryHandler(_unitOfWorks, _mapper);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PaginatedResult<InterviewViewModel>>());
        Assert.That(result.Items, Is.Empty);
    }

    [Test]
    public async Task InterviewSendReminderCommand_WithInvitedStatus_ReturnsUpdatedInterview()
    {
        var existedInterview = await _context.Interviews.FirstAsync();
        var interviewer = existedInterview.Interviewers!.First();

        var command = new InterviewRemindCommand
        {
            Email = interviewer.Email!,
            InterviewId = existedInterview.Id,
            InterviewLink = "https://ims.example.com",
        };

        var handler = new InterviewRemindCommandHandler
            (_unitOfWorks, _mapper, _emailService.Object, _backgroundService.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.True);
        Assert.That(_context.Reminders.ToListAsync().Result, Is.Not.Empty);
        Assert.That(_context.Reminders.AnyAsync(r => r.Email == command.Email).Result, Is.True);
    }
}