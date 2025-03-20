using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMS.Business.DTOs;
using IMS.Business.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Handlers;

public class LoginCommandHandler(
    ITokenService tokenService
) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly ITokenService _tokenService = tokenService;
    
    // TODO: Replace hardcoded values when code can work with database
    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // TODO: Check if user exists on database
        if (!request.Username.Equals("admin"))
        {
            throw new ArgumentException("User with username not found");
        }

        // TODO: Check for correct password saved on database
        if (!request.Password.Equals("password"))
        {
            throw new ArgumentException("Password is incorrect");
        }

        // TODO: Remove this after code can work with database
        var tempId = Guid.NewGuid();

        // TODO: Add user information from database instead
        var userInfo = new UserInfo
        {
            Id = tempId,
            Username = request.Username,
            DisplayName = "Temporary User",
            Email = "temp@domain.com",
            Roles = ["Admin"]
        };

        var accessToken = await _tokenService.GenerateAccessTokenAsync(tempId, request.Username);
        var tokenHandler = new JwtSecurityTokenHandler();

        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(tempId);

        // Temporary save access token in a file
        await File.WriteAllTextAsync("token.txt", tokenHandler.WriteToken(accessToken), cancellationToken);

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
