using System.Linq.Expressions;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class ForgotPasswordHandlerTest
{
    private Mock<IEmailService> _emailServiceMock;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<UserManager<User>> _userManagerMock;
    private ForgotPasswordCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _emailServiceMock = new Mock<IEmailService>();
        _tokenServiceMock = new Mock<ITokenService>();

        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), // Cần thiết cho UserManager
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new List<IUserValidator<User>>(), // Tránh null
            new List<IPasswordValidator<User>>(), // Tránh null
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
        );

        _handler = new ForgotPasswordCommandHandler(
            _emailServiceMock.Object,
            _tokenServiceMock.Object,
            _userManagerMock.Object
        );
    }

    [Test]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var request = new ForgotPasswordCommand { Email = "notfound@example.com" };

        // Mock UserManager để luôn trả về null khi tìm email
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((User)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await _handler.Handle(request, CancellationToken.None)
        );

        Assert.That(exception.Message, Is.EqualTo("This email is not linked to any account."));
    }

    [Test]
    public async Task Handle_ShouldSendEmail_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        var request = new ForgotPasswordCommand { Email = "test@example.com" };

        // Mock UserManager để trả về user khi tìm kiếm theo email
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        // Mock TokenService để trả về token đặt lại mật khẩu
        _tokenServiceMock.Setup(x => x.GenerateResetPasswordTokenAsync(user.Id))
            .ReturnsAsync(new ResetPasswordToken 
            { 
                Token = "resetToken",
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                UserId = 1
            });

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert - Kiểm tra email đã được gửi đúng
        _emailServiceMock.Verify(x => x.SendEmailAsync(
            "test@example.com",
            "Password Reset",
            It.Is<string>(body => body.Contains("resetToken"))),
            Times.Once);
    }



}
