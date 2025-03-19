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

        // TODO: Consider adding user information from database and add to token
        var userInfo = string.Empty;

        // TODO: Remove this after code can work with database
        var tempId = Guid.NewGuid().ToString();

        var token = await _tokenService.GenerateAccessTokenAsync(tempId, request.Username);
        var tokenHandler = new JwtSecurityTokenHandler();

        // Temporary save access token in a file
        await File.WriteAllTextAsync("token.txt", tokenHandler.WriteToken(token), cancellationToken);

        var loginResult = new LoginResultDto
        {
            AccessToken = tokenHandler.WriteToken(token),
            UserId = tempId,
            UserInfo = userInfo,
            ExpiresAt = token.ValidTo
        };

        return loginResult;
    }
}
