using System.IdentityModel.Tokens.Jwt;

using IMS.Business.DTOs;
using IMS.Business.Services;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace IMS.Business.Handlers;

public class LoginCommandHandler(
    ITokenService tokenService,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly ITokenService _tokenService = tokenService;

    private readonly UserManager<User> _userManager = userManager;

    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username)
            ?? throw new ArgumentException("User with username not found");
        
        var isCorrectPassword = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!isCorrectPassword.Succeeded)
        {
            throw new ArgumentException("Password is incorrect");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var userInfo = new UserInfo
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            DisplayName = user.FullName,
            Email = user.Email,
            Roles = [.. roles]
        };

        var accessToken = await _tokenService.GenerateAccessTokenAsync(user.Id);
        var tokenHandler = new JwtSecurityTokenHandler();

        await _tokenService.RevokeAllRefreshTokenAsync(user.Id);

        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

        var loginResult = new LoginResultDto
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            RefreshToken = refreshToken.Token,
            UserInfo = userInfo,
            ExpiresAt = accessToken.ValidTo
        };

        return loginResult;
    }
}
