using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Services;

public class TokenService(
    IConfiguration configuration,
    IUnitOfWorks unitOfWorks,
    UserManager<User> userManager) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;

    private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

    private readonly UserManager<User> _userManager = userManager;


    public async Task<JwtSecurityToken> GenerateAccessTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

    var roles = user.UserRoles?.Select(x => x.Role) ?? [];

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role?.RoleName ?? string.Empty)));

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

    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
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
        
        _unitOfWorks.RefreshTokenRepository.Add(refreshToken);

        return refreshToken;
    }

    public async Task<ResetPasswordToken> GenerateResetPasswordTokenAsync(int userId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        var resetToken = new ResetPasswordToken
        {
            Token = token,
            UserId = userId,
            ExpiryDate = DateTime.Now.AddDays(1),
            IsRevoked = false,
        };
        
        _unitOfWorks.ResetPasswordTokenRepository.Add(resetToken);

        return resetToken;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid refresh token");
        }

        var tokens = await _unitOfWorks.RefreshTokenRepository.GetAllAsync();

        var revokeToken = tokens.FirstOrDefault(x => x.Token == token);

        if (revokeToken == null)
        {
            throw new ArgumentException("Refresh token not found");
        }

        revokeToken.IsRevoked = true;

        _unitOfWorks.RefreshTokenRepository.Update(revokeToken);

        return true;
    }

    public async Task<bool> RevokeResetPasswordTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid token");
        }

        var tokens = await _unitOfWorks.ResetPasswordTokenRepository.GetAllAsync();

        var revokeToken = tokens.FirstOrDefault(x => x.Token == token);

        if (revokeToken == null)
        {
            throw new ArgumentException("Token not found");
        }

        revokeToken.IsRevoked = true;

        _unitOfWorks.ResetPasswordTokenRepository.Update(revokeToken);

        return true;
    }
}