using IMS.API.Controllers;
using IMS.Business.DTOs;
using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace IMS.Tests.AuthTest;

[TestFixture]
public class LoginHandlerTest
{
    private Mock<ITokenService> _mockTokenService;
    private Mock<UserManager<User>> _mockUserManager;
    private Mock<SignInManager<User>> _mockSignInManager;
    private LoginCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockUserManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new List<IUserValidator<User>>(), // Tránh null
            new List<IPasswordValidator<User>>(), // Tránh null
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
        );

        // Mock SignInManager
        _mockSignInManager = new Mock<SignInManager<User>>(
            _mockUserManager.Object, 
            new Mock<IHttpContextAccessor>().Object, 
            new Mock<IUserClaimsPrincipalFactory<User>>().Object, 
            new Mock<IOptions<IdentityOptions>>().Object, 
            new Mock<ILogger<SignInManager<User>>>().Object
        );

        _mockTokenService = new Mock<ITokenService>();
        _handler = new LoginCommandHandler(_mockTokenService.Object, _mockUserManager.Object,_mockSignInManager.Object);
    }

    [Test]
    public async Task Handle_ValidLogin_ReturnsLoginResultDto()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com", FullName = "Test User" };
        var command = new LoginCommand { Username = "testuser", Password = "password123" };
        var roles = new List<string> { "Admin" };

        _mockUserManager.Setup(u => u.FindByNameAsync(command.Username))
                        .ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, command.Password))
                        .ReturnsAsync(true);
        _mockUserManager.Setup(u => u.GetRolesAsync(user))
                        .ReturnsAsync(roles);

        var fakeAccessToken = new JwtSecurityToken();
        var fakeRefreshToken = new RefreshToken { Token = "refresh-token-123" };

        _mockTokenService.Setup(t => t.GenerateAccessTokenAsync(user.Id))
                         .ReturnsAsync(fakeAccessToken);
        _mockTokenService.Setup(t => t.GenerateRefreshTokenAsync(user.Id))
                         .ReturnsAsync(fakeRefreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.RefreshToken, Is.EqualTo("refresh-token-123"));
        Assert.That(result.UserInfo.Username, Is.EqualTo("testuser"));
        Assert.That(result.UserInfo.Roles, Has.Exactly(1).EqualTo("Admin"));
    }

    [Test]
    public void Handle_InvalidUsername_ThrowsArgumentException()
    {
        // Arrange
        var command = new LoginCommand { Username = "invaliduser", Password = "password123" };
        _mockUserManager.Setup(u => u.FindByNameAsync(command.Username))
                        .ReturnsAsync((User)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("User with username not found"));
    }

    [Test]
    public async Task Handle_InvalidPassword_ThrowsArgumentException()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "testuser", Email = "test@example.com", FullName = "Test User" };
        var command = new LoginCommand { Username = "testuser", Password = "wrongpassword" };

        _mockUserManager.Setup(u => u.FindByNameAsync(command.Username))
                        .ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, command.Password))
                        .ReturnsAsync(false); // Sai mật khẩu

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Password is incorrect"));
    }
}
