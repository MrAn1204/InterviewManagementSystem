
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers
{
    // UpdateUserCommandHandler.cs
public class UpdateUserCommandHandler(UserManager<User> _userManager, IUnitOfWorks _unitOfWork) : IRequestHandler<UpdateUserCommand, Unit>
{

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) 
            throw new Exception("User not found");
    
        if (request.Roles == null || request.Roles.Length == 0)
                throw new ArgumentException("At least one role is required");   
        
        user.Email = request.Email;
        user.FullName = request.FullName != null ? request.FullName : "N/A";
        user.DOB = request.DOB;
        user.Address = request.Address;
        user.PhoneNumber = request.PhoneNumber;
        user.DepartmentId = request.DepartmentId;
        user.Note = request.Note;
        user.Gender = request.Gender;
        user.UpdatedDate = DateTime.Now;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRolesAsync(user, request.Roles);

        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}

   
}