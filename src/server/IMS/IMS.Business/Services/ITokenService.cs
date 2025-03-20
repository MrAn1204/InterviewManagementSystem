using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Models.Entities;

namespace IMS.Business.Services;

public interface ITokenService
{
	// TODO: Replace parameters with User
	Task<JwtSecurityToken> GenerateAccessTokenAsync(Guid tempId, string tempUsername);

    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);

	Task<bool> RevokeRefreshTokenAsync(string token);
}
