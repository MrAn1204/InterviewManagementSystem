using System.IdentityModel.Tokens.Jwt;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Data;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class RefreshTokenHandlerTests
{
    private ApplicationDbContext _dbContext;
    private IUnitOfWorks _unitOfWork;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<UserManager<User>> _userManagerMock;
    private RefreshTokenCommandHandler _handler;
    private User _testUser;
    private RefreshToken _validRefreshToken;

    [SetUp]
    public void Setup()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"RefreshTokenTestDb_{Guid.NewGuid()}")
            .Options;
        
        _dbContext = new ApplicationDbContext(options);
        _unitOfWork = new UnitOfWorks(_dbContext);

        // Create test user
        _testUser = new User { Id = 1, FullName = "Nguyen Tat Loc", UserName = "testuser", Email = "test@example.com" };
        _dbContext.Users.Add(_testUser);

        // Create valid refresh token
        _validRefreshToken = new RefreshToken
        {
            Token = "valid_token",
            UserId = _testUser.Id,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            IsRevoked = false
        };
        _dbContext.RefreshTokens.Add(_validRefreshToken);
        _dbContext.SaveChanges();

        // Mock UserManager
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<User>>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<User>>>());
        
        _userManagerMock.Setup(x => x.FindByIdAsync(_testUser.Id.ToString()))
            .ReturnsAsync(_testUser);
        
        _userManagerMock.Setup(x => x.GetRolesAsync(_testUser))
            .ReturnsAsync(new List<string> { "User" });

        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new RefreshTokenCommandHandler(_unitOfWork, _tokenServiceMock.Object, _userManagerMock.Object);
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

    // 1. Test case for successful scenario
    [Test]
    public async Task Handle_WithValidTokenAndUser_ReturnsLoginResult()
    {
        // Arrange
        var request = new RefreshTokenCommand { RefreshToken = _validRefreshToken.Token };
        var testToken = new JwtSecurityToken(expires: DateTime.UtcNow.AddHours(1));
        _tokenServiceMock.Setup(x => x.GenerateAccessTokenAsync(_testUser.Id))
            .ReturnsAsync(testToken);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccessToken, Is.Not.Null);
        Assert.That(result.RefreshToken, Is.EqualTo(_validRefreshToken.Token));
        Assert.That(result.UserInfo.Username, Is.EqualTo(_testUser.UserName));
    }

    // 2. Test case for invalid refresh token (null/expired/revoked)
    [Test]
    public void Handle_WithInvalidRefreshToken_ThrowsUnauthorizedException()
    {
        // Arrange - create expired token
        var expiredToken = new RefreshToken
        {
            Token = "expired_token",
            UserId = _testUser.Id,
            ExpiryDate = DateTime.UtcNow.AddHours(-1),
            IsRevoked = false
        };
        _dbContext.RefreshTokens.Add(expiredToken);
        _dbContext.SaveChanges();

        var request = new RefreshTokenCommand { RefreshToken = expiredToken.Token };

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
            _handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Invalid refresh token"));
    }

    // 3. Test case for invalid user
    [Test]
    public void Handle_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new RefreshTokenCommand { RefreshToken = _validRefreshToken.Token };
        _userManagerMock.Setup(x => x.FindByIdAsync(_testUser.Id.ToString()))
            .ReturnsAsync((User)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
            _handler.Handle(request, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Invalid user"));
    }
}