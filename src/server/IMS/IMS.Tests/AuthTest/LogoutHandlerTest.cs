using IMS.Business.Handlers;
using IMS.Business.Services;
using Moq;
using NUnit.Framework;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class LogoutHandlerTest
{
    private Mock<ITokenService> _mockTokenService;
    private LogoutCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockTokenService = new Mock<ITokenService>();
        _handler = new LogoutCommandHandler(_mockTokenService.Object);
    }

    [Test]
    public async Task Handle_ValidRefreshToken_CallsRevokeRefreshTokenAsync()
    {
        // Arrange
        var command = new LogoutCommand { RefreshToken = "valid-refresh-token" };

        _mockTokenService.Setup(t => t.RevokeRefreshTokenAsync(command.RefreshToken))
                         .ReturnsAsync(true)
                         .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.True);
        _mockTokenService.Verify(t => t.RevokeRefreshTokenAsync(command.RefreshToken), Times.Once);
    }

    [Test]
    public void Handle_NullOrEmptyToken_ThrowsArgumentException()
    {
        // Arrange
        var command = new LogoutCommand { RefreshToken = "" }; // Test với rỗng

        _mockTokenService.Setup(t => t.RevokeRefreshTokenAsync(command.RefreshToken))
                         .ThrowsAsync(new ArgumentException("Invalid refresh token"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Invalid refresh token"));

        _mockTokenService.Verify(t => t.RevokeRefreshTokenAsync(command.RefreshToken), Times.Once);
    }

    [Test]
    public void Handle_InvalidRefreshToken_ThrowsArgumentException()
    {
        // Arrange
        var command = new LogoutCommand { RefreshToken = "invalid-refresh-token" };

        _mockTokenService.Setup(t => t.RevokeRefreshTokenAsync(command.RefreshToken))
                         .ThrowsAsync(new ArgumentException("Refresh token not found"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Refresh token not found"));

        _mockTokenService.Verify(t => t.RevokeRefreshTokenAsync(command.RefreshToken), Times.Once);
    }
}
