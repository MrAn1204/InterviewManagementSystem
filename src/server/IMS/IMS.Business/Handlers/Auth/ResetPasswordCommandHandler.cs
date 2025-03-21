using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class ResetPasswordCommandHandler(UserManager<User> userManager) : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.NewPassword.Equals(request.ConfirmNewPassword))
        {
            throw new ArgumentException("Passwords do not match");
        }

        var user = await _userManager.Users.FirstAsync(
            x => x.Email == request.Email, cancellationToken);
        
        user.Password = request.NewPassword;

        // UpdateAsync() requires SecurityStamp. Assign random value since user is not created with it.
        user.SecurityStamp ??= Guid.NewGuid().ToString();
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}