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
	Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);

    Task<RefreshToken> GenerateRefreshTokenAsync(int userId);

    Task<ResetPasswordToken> GenerateResetPasswordTokenAsync(User user);

	Task<bool> RevokeRefreshTokenAsync(string token);

	Task<bool> MarkUsedResetPasswordTokenAsync(string token);

	Task<bool> ValidateResetPasswordAsync(string token);
}
