using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMS.Business.DTOs;
using IMS.Business.Services;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Handlers;

public class LoginCommandHandler(
    ITokenService tokenService,
    UserManager<User> userManager
) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly ITokenService _tokenService = tokenService;

    private readonly UserManager<User> _userManager = userManager;

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(
            x => x.UserName == request.Username, cancellationToken: cancellationToken)
            ?? throw new ArgumentException("User with username not found");
        
        // No hashed password
        if (user.Password != request.Password)
        {
            throw new ArgumentException("Password is incorrect");
        }

        var roles = user.UserRoles ?? [];

        var userInfo = new UserInfo
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            DisplayName = user.FullName,
            Email = user.Email,
            Roles = [.. roles.Select(x => x.Role?.RoleName ?? string.Empty)]
        };

        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();

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
