
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers;

public class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWorks _unitOfWork;

    public ActiveUserCommandHandler(UserManager<User> userManager, IUnitOfWorks unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Đánh dấu user là Inactive
        user.IsActive = true;
        user.UpdatedDate = DateTime.Now;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw new Exception(updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to inactivate user");
        }

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}

