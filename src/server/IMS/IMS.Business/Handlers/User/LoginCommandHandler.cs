using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMS.Business.DTOs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Handlers;

public class LoginCommandHandler(IConfiguration configuration) : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IConfiguration _configuration = configuration;
    
    // TODO: Replace hardcoded values when code can work with database
    public Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
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

        // TODO: Add more information from database
        var claims = new List<Claim>
        {
            new("id", tempId),
            new("username", request.Username),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration[_configuration["Jwt:ValidIssuer"]!],
            audience: _configuration[_configuration["Jwt:ValidAudience"]!],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            
        );

        // Temporary save access token in a file
        File.WriteAllText("token.txt", new JwtSecurityTokenHandler().WriteToken(token));

        var loginResult = new LoginResultDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = tempId,
            UserInfo = userInfo,
            ExpiresAt = token.ValidTo,
            IssuedAt = token.ValidFrom,
        };

        return Task.FromResult(loginResult);
    }
}
