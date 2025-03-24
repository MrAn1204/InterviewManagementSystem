using System.IdentityModel.Tokens.Jwt;
using IMS.Business.DTOs;
using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers;

public class RefreshTokenCommandHandler(
    IUnitOfWorks unitOfWorks,
    ITokenService tokenService,
    UserManager<User> userManager
) : IRequestHandler<RefreshTokenCommand, LoginResultDto>
{
    private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

    private readonly ITokenService _tokenService = tokenService;

    private readonly UserManager<User> _userManager = userManager;

    public async Task<LoginResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _unitOfWorks.RefreshTokenRepository.GetQuery()
            .FirstOrDefault(x => x.Token == request.RefreshToken);

        if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow || refreshToken.IsRevoked)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString())
            ?? throw new UnauthorizedAccessException("Invalid user");
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var userInfo = new UserInfo
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            DisplayName = user.FullName,
            Email = user.Email,
            Roles = [.. roles]
        };

        var accessToken = await _tokenService.GenerateAccessTokenAsync(refreshToken.UserId);
        
        var tokenHandler = new JwtSecurityTokenHandler();

        var loginResult = new LoginResultDto
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            RefreshToken = refreshToken.Token,
            UserInfo = userInfo,
            ExpiresAt = accessToken.ValidTo
        };

        return loginResult;
    }
}