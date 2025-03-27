using IMS.API.Controllers;
using IMS.Business.DTOs;
using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IMS.API.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _controller;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Test]
        public async Task Login_WithValidCredentials_ReturnsTokenResponse()
        {
            // Arrange
            var loginDto = new LoginCommand { Username = "nguyenvana", Password = "Abc@123" };
            var expectedUserInfo = new UserInfo
            {
                Id = 1,
                Username = "nguyenvana",
                DisplayName = "Nguyễn Văn A",
                Email = "vana@example.com",
                Roles = new[] { "Admin", "Manager" }
            };

            var expectedResult = new LoginResultDto
            {
                AccessToken = "access_token_123",
                RefreshToken = "refresh_token_456",
                UserInfo = expectedUserInfo,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await _controller.Login(loginDto);

            // Assert
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
            
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<LoginResultDto>());
            
            var result = okResult.Value as LoginResultDto;
            
            // Verify all required fields are populated
            Assert.That(result.AccessToken, Is.EqualTo("access_token_123"));
            Assert.That(result.RefreshToken, Is.EqualTo("refresh_token_456"));
            Assert.That(result.UserInfo, Is.Not.Null);
            Assert.That(result.UserInfo.Username, Is.EqualTo("nguyenvana"));
            Assert.That(result.ExpiresAt, Is.GreaterThan(DateTime.UtcNow));
        }

        [Test]
        public async Task Login_WithInvalidCredentials_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginCommand { Username = "wrong", Password = "wrong" };
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid credentials"));

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _controller.Login(loginDto));
        }

        [Test]
        public async Task Login_WithMissingRequiredFields_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Username is required");
            _controller.ModelState.AddModelError("Password", "Password is required");
            
            var invalidDto = new LoginCommand { Username = "", Password = "" };

            // Act
            var result = await _controller.Login(invalidDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}