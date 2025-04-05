using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers;

public class InactiveUserCommandHandler : IRequestHandler<InactiveUserCommand, Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWorks _unitOfWork;

    public InactiveUserCommandHandler(UserManager<User> userManager, IUnitOfWorks unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(InactiveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Đánh dấu user là Inactive
        user.IsActive = false;

        // Cập nhật user qua UserManager
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw new Exception(updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to inactivate user");
        }

        // Lưu thay đổi nếu cần qua UnitOfWorks (nếu bạn dùng pattern này cùng lúc với UserManager)
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}

