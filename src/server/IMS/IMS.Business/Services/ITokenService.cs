using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
