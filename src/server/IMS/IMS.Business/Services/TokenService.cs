
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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


    public async Task<JwtSecurityToken> GenerateAccessTokenAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user!.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role ?? string.Empty)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            audience: _configuration["Jwt:ValidAudience"],
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
        await _unitOfWorks.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<ResetPasswordToken> GenerateResetPasswordTokenAsync(int userId)
    {       
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetToken = new ResetPasswordToken
        {
            Token = token,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
        };
        
        _unitOfWorks.ResetPasswordTokenRepository.Add(resetToken);
        await _unitOfWorks.SaveChangesAsync();

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

    public async Task<int> RevokeAllRefreshTokenAsync(int userId)
    {
        var activeTokens = await _unitOfWorks.Context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();
        
        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
        }
        
        return await _unitOfWorks.SaveChangesAsync();
    }

    public async Task<bool> MarkUsedResetPasswordTokenAsync(string token)
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

        revokeToken.IsUsed = true;

        _unitOfWorks.ResetPasswordTokenRepository.Update(revokeToken);

        return true;
    }

    public async Task<bool> ValidateResetPasswordAsync(string token)
    {
        var resetToken = await _unitOfWorks.ResetPasswordTokenRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Token == token);

        return resetToken != null && resetToken.ExpiryDate > DateTime.UtcNow && !resetToken.IsUsed;
    }
}