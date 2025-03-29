using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Data;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class ResetPasswordHandlerTests
{
    private ApplicationDbContext _dbContext;
    private IUnitOfWorks _unitOfWork;
    private User _testUser;
    private ResetPasswordToken _testToken;
    private string _databaseName;
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<ITokenService> _tokenServiceMock;

    [SetUp]
    public void Setup()
    {
        // Create a unique database name for each test
        _databaseName = $"ViVuStoreTest_{Guid.NewGuid()}";
        
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;
        
        _dbContext = new ApplicationDbContext(options);
        
        // Create test user
        _testUser = new User
        {
            Id = 1,
            FullName = "Nguyen Tat Loc",
            UserName = "testuser@example.com",
            Email = "testuser@example.com"
        };
        
        _dbContext.Users.Add(_testUser);
        
        // Create test token
        _testToken = new ResetPasswordToken
        {
            Token = "valid_token_123",
            UserId = _testUser.Id,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsUsed = false
        };
        
        _dbContext.ResetPasswordTokens.Add(_testToken);
        _dbContext.SaveChanges();
        
        // Mock UserManager
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new List<IUserValidator<User>>(),
            new List<IPasswordValidator<User>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        
        // Setup successful password reset
        _userManagerMock.Setup(x => x.ResetPasswordAsync(
                It.IsAny<User>(), 
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock.Setup(x => x.FindByIdAsync(_testUser.Id.ToString()))
            .ReturnsAsync(_testUser);
        
        // Mock TokenService
        _tokenServiceMock = new Mock<ITokenService>();
        _tokenServiceMock.Setup(x => x.MarkUsedResetPasswordTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        
        // Create UnitOfWork with real DbContext
        _unitOfWork = new UnitOfWorks(_dbContext);
    }

    [TearDown]
public void TearDown()
{
    try
    {
        // Đảm bảo xóa database trước
        _dbContext?.Database?.EnsureDeleted();
        
        // Giải phóng UnitOfWork
        if (_unitOfWork is IDisposable unitOfWorkDisposable)
        {
            unitOfWorkDisposable.Dispose();
        }
        
        // Giải phóng DbContext
        _dbContext?.Dispose();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during teardown: {ex.Message}");
        throw; // Re-throw để test fail rõ ràng
    }
}

    [Test]
    public async Task Handle_WithValidTokenAndMatchingPasswords_ResetsPasswordSuccessfully()
    {
        // Arrange
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object,
            _unitOfWork,
            _tokenServiceMock.Object);
        
        var request = new ResetPasswordCommand
        {
            Token = WebUtility.UrlEncode(_testToken.Token),
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
        _userManagerMock.Verify(x => x.ResetPasswordAsync(
            _testUser, _testToken.Token, request.NewPassword), Times.Once);
        _tokenServiceMock.Verify(x => x.MarkUsedResetPasswordTokenAsync(_testToken.Token), Times.Once);
    }

    [Test]
    public void Handle_WithNonMatchingPasswords_ThrowsArgumentException()
    {
        // Arrange
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object,
            _unitOfWork,
            _tokenServiceMock.Object);
        
        var request = new ResetPasswordCommand
        {
            Token = WebUtility.UrlEncode(_testToken.Token),
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "DifferentPassword456!"
        };
        
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Passwords do not match"));
    }

    [Test]
    public void Handle_WithInvalidToken_ThrowsInvalidOperationException()
    {
        // Arrange
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object,
            _unitOfWork,
            _tokenServiceMock.Object);
        
        var request = new ResetPasswordCommand
        {
            Token = WebUtility.UrlEncode("invalid_token"),
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };
        
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Reset password token is not found."));
    }

    [Test]
    public void Handle_WithExpiredToken_ThrowsInvalidOperationException()
    {
        // Arrange
        var expiredToken = new ResetPasswordToken
        {
            Token = "expired_token",
            UserId = _testUser.Id,
            ExpiryDate = DateTime.UtcNow.AddHours(-1),
            IsUsed = false
        };
        
        _dbContext.ResetPasswordTokens.Add(expiredToken);
        _dbContext.SaveChanges();
        
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object,
            _unitOfWork,
            _tokenServiceMock.Object);
        
        var request = new ResetPasswordCommand
        {
            Token = WebUtility.UrlEncode(expiredToken.Token),
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };
        
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("This token has already been used or expired."));
    }

    [Test]
    public void Handle_WithUsedToken_ThrowsInvalidOperationException()
    {
        // Arrange
        var usedToken = new ResetPasswordToken
        {
            Token = "used_token",
            UserId = _testUser.Id,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsUsed = true
        };
        
        _dbContext.ResetPasswordTokens.Add(usedToken);
        _dbContext.SaveChanges();
        
        var handler = new ResetPasswordCommandHandler(
            _userManagerMock.Object,
            _unitOfWork,
            _tokenServiceMock.Object);
        
        var request = new ResetPasswordCommand
        {
            Token = WebUtility.UrlEncode(usedToken.Token),
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };
        
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("This token has already been used or expired."));
    }
}