
using System.IdentityModel.Tokens.Jwt;

using IMS.Domain.Entities;

namespace IMS.Business.Services;

public interface ITokenService
{
	Task<JwtSecurityToken> GenerateAccessTokenAsync(int userId);

    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);

    Task<ResetPasswordToken> GenerateResetPasswordTokenAsync(int userId);

	Task<bool> RevokeRefreshTokenAsync(string token);

	Task<int> RevokeAllRefreshTokenAsync(int userId);

	Task<bool> MarkUsedResetPasswordTokenAsync(string token);

	Task<bool> ValidateResetPasswordAsync(string token);
}
