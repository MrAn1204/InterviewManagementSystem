using System.Net;
using System.Web;
using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class ResetPasswordCommandHandler(
    UserManager<User> userManager,
    IUnitOfWorks unitOfWorks
) : IRequestHandler<ResetPasswordCommand, string>
{
    private readonly UserManager<User> _userManager = userManager;

    private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

    public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.NewPassword.Equals(request.ConfirmNewPassword))
        {
            throw new ArgumentException("Passwords do not match");
        }

        var decodedToken = WebUtility.UrlDecode(request.Token);
        decodedToken = decodedToken.Replace(" ", "+");

        var resetToken = await _unitOfWorks.ResetPasswordTokenRepository.GetQuery()
            .FirstOrDefaultAsync(x => x.Token == decodedToken, cancellationToken)
            ?? throw new InvalidOperationException("Reset password token is not found.");
        
        if (resetToken.IsUsed || resetToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("This token has already been used or expired.");
        }

        var user = await _userManager.FindByIdAsync(resetToken.UserId.ToString());
        
        var result = await _userManager.ResetPasswordAsync(user!, resetToken.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            return result.Errors.First().Description;
        }

        return string.Empty;
    }
}