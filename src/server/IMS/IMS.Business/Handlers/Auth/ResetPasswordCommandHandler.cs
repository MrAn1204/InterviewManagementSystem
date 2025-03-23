using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class ResetPasswordCommandHandler(
    UserManager<User> userManager,
    IUnitOfWorks unitOfWorks,
    ITokenService tokenService
) : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager = userManager;

    private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

    private readonly ITokenService _tokenService = tokenService;

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.NewPassword.Equals(request.ConfirmNewPassword))
        {
            throw new ArgumentException("Passwords do not match");
        }

        var resetToken = await _unitOfWorks.ResetPasswordTokenRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Token == request.Token, cancellationToken);
        
        if (resetToken == null || resetToken.IsRevoked)
        {
            throw new InvalidOperationException("Reset password token is not found or is revoked.");
        }

        var user = await _userManager.FindByIdAsync(resetToken.UserId.ToString());
        user!.Password = request.NewPassword;

        // UpdateAsync() requires SecurityStamp. Assign random value since user is not created with it.
        user.SecurityStamp ??= Guid.NewGuid().ToString();
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return false;
        }
    
        await _tokenService.RevokeResetPasswordTokenAsync(request.Token);

        return true;
    }
}