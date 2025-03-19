using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;

	// TODO: Replace parameters with User
    public async Task<JwtSecurityToken> GenerateAccessTokenAsync(string tempId, string tempUsername)
    {
        // TODO: Add more information from database
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, tempId),
            new(JwtRegisteredClaimNames.UniqueName, tempUsername),
        };

		// TODO: Add role claims

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration[_configuration["Jwt:ValidIssuer"]!],
            audience: _configuration[_configuration["Jwt:ValidAudience"]!],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

		return token;
    }
}