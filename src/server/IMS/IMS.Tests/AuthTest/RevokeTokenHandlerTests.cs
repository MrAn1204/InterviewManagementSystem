using IMS.Business.Handlers;
using IMS.Business.Services;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class RevokeTokenHandlerTests
{
    private Mock<ITokenService> _tokenServiceMock;
    private RevokeTokenCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new RevokeTokenCommandHandler(_tokenServiceMock.Object);
    }

    // 1. Trường hợp thành công
    [Test]
    public async Task Handle_WithValidToken_ReturnsTrue()
    {
        // Arrange
        var validToken = "valid_token_123";
        var request = new RevokeTokenCommand { RefreshToken = validToken };
        
        _tokenServiceMock.Setup(x => x.RevokeRefreshTokenAsync(validToken))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.True, "Should return true for successful revocation");
        _tokenServiceMock.Verify(x => x.RevokeRefreshTokenAsync(validToken), Times.Once);
    }

    // 2. Trường hợp token rỗng
    [Test]
    public void Handle_WithEmptyToken_ThrowsArgumentException()
    {
        // Arrange
        var emptyToken = "";
        var request = new RevokeTokenCommand { RefreshToken = emptyToken };
        
        _tokenServiceMock.Setup(x => x.RevokeRefreshTokenAsync(emptyToken))
            .ThrowsAsync(new ArgumentException("Invalid refresh token"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => 
            _handler.Handle(request, CancellationToken.None));
        
        Assert.That(ex.Message, Is.EqualTo("Invalid refresh token"));
    }

    // 3. Trường hợp token không tồn tại
    [Test]
    public void Handle_WhenTokenNotFound_ThrowsArgumentException()
    {
        // Arrange
        var nonExistentToken = "non_existent_token";
        var request = new RevokeTokenCommand { RefreshToken = nonExistentToken };
        
        _tokenServiceMock.Setup(x => x.RevokeRefreshTokenAsync(nonExistentToken))
            .ThrowsAsync(new ArgumentException("Refresh token not found"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => 
            _handler.Handle(request, CancellationToken.None));
        
        Assert.That(ex.Message, Is.EqualTo("Refresh token not found"));
    }
}