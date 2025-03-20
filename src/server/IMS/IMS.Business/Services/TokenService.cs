using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;

	// TODO: Replace parameters with User
    public async Task<JwtSecurityToken> GenerateAccessTokenAsync(Guid tempId, string tempUsername)
    {
        // TODO: Add more information from database
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, tempId.ToString()),
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

    public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        var duration = Convert.ToDouble(_configuration["Jwt:RefreshDuration"]!);

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            IsRevoked = false,
            ExpiryDate = DateTime.Now.AddDays(duration)
        };

        // TODO: Save refresh token to database instead
        await File.WriteAllTextAsync("refreshToken.txt", refreshToken.Token);

        return refreshToken;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid refresh token");
        }

        // TODO: Also check if refresh token exists on database
        if (!File.Exists("refreshToken.txt"))
        {
            throw new ArgumentException("Refresh token not found");
        }

        // TODO: Set IsRevoked to true on database instead
        File.Delete("refreshToken.txt");

        return true;
    }
}