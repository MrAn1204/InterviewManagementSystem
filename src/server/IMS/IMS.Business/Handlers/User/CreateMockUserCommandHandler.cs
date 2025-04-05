
using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace IMS.Business.Handlers;

public class CreateMockUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IUnitOfWorks unitOfWork
    // ,
    // IEmailService emailService
    ) : IRequestHandler<CreateMockUserCommand, string>
{
    public async Task<string> Handle(CreateMockUserCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync();
        
        try
        {
            // Validate department
            var department = await unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId);
            if (department == null)
                return "Invalid department";

            // Validate roles
            var validRoles = await roleManager.Roles
                .Where(r => request.Roles.Contains(r.Name))
                .Select(r => r.Name)
                .ToListAsync(cancellationToken);

            if (validRoles.Count != request.Roles.Length)
                return "One or more roles are invalid";

            // Create user
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                DepartmentId = request.DepartmentId,
                Address = request.Address,
                DOB = request.DOB,
                PhoneNumber = request.PhoneNumber,
                Note = request.Note,
                Gender = request.Gender ,// Gán giá trị gender
                IsActive = (bool)request.IsActive ? true : false
            };

            var createResult = await userManager.CreateAsync(user, request.Password);
            
            if (!createResult.Succeeded)
                return createResult.Errors.First().Description;

            // Assign roles
            await userManager.AddToRolesAsync(user, validRoles);

            // Send email
            //await emailService.SendUserCreatedEmailAsync(user.Email, user.UserName, request.Password);

            await unitOfWork.CommitTransactionAsync();
            return string.Empty;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return ex.Message;
        }
    }
}