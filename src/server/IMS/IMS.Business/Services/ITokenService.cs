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

	Task<bool> RevokeRefreshTokenAsync(string token);
}
