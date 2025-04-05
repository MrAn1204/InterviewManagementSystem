using IMS.Business.Handlers;
using IMS.Business.Services;
using Moq;
using NUnit.Framework;

namespace IMS.Tests.AuthTest;

public class ValidateResetPasswordHandlerTests
{
     private Mock<ITokenService> _tokenServiceMock;
    private ValidateResetPasswordCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ValidateResetPasswordCommandHandler(_tokenServiceMock.Object);
    }

    [Test]
    public async Task Handle_WithValidToken_ReturnsTrue()
    {
        // Arrange
        var validToken = "valid_token_123";
        var request = new ValidateResetPasswordCommand { Token = validToken };
        
        _tokenServiceMock.Setup(x => x.ValidateResetPasswordAsync(validToken))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.True);
        _tokenServiceMock.Verify(x => x.ValidateResetPasswordAsync(validToken), Times.Once);
    }

    [Test]
    public async Task Handle_WithInvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "invalid_token_456";
        var request = new ValidateResetPasswordCommand { Token = invalidToken };
        
        _tokenServiceMock.Setup(x => x.ValidateResetPasswordAsync(invalidToken))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.False);
        _tokenServiceMock.Verify(x => x.ValidateResetPasswordAsync(invalidToken), Times.Once);
    }
}
