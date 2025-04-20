
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace IMS.Business.Handlers;

public class CreateMockUserCommandHandler
    : IRequestHandler<CreateMockUserCommand, UserDetailViewModel>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMockUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IUnitOfWorks unitOfWork,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDetailViewModel> Handle(CreateMockUserCommand request, CancellationToken cancellationToken)
    {
        using var tx = await _unitOfWork.BeginTransactionAsync();
        // 1. Validate department
        var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(request.DepartmentId);
        if (dept == null) throw new ArgumentException("Invalid department");

        // 2. Validate roles
        var validRoles = await _roleManager.Roles
            .Where(r => request.Roles.Contains(r.Name))
            .Select(r => r.Name)
            .ToListAsync(cancellationToken);
        if (validRoles.Count != request.Roles.Length)
            throw new ArgumentException("One or more roles are invalid");

        // 3. Generate unique username from email prefix
        var baseName = request.Email.Split('@')[0].Replace(".", "").Replace(" ", "");
        var username = baseName;
        int suffix = 1;
        while (await _userManager.FindByNameAsync(username) != null)
        {
            username = $"{baseName}{suffix++}";
        }

        // 4. Create user
        var user = new User
        {
            UserName = username,
            Email = request.Email,
            FullName = request.FullName,
            DepartmentId = request.DepartmentId,
            Address = request.Address,
            DOB = request.DOB,
            PhoneNumber = request.PhoneNumber,
            Note = request.Note,
            Gender = request.Gender,
            IsActive = request.IsActive ?? true
        };
        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            throw new InvalidOperationException(createResult.Errors.First().Description);

        // 5. Assign roles
        await _userManager.AddToRolesAsync(user, validRoles);

        // 6. Commit
        await _unitOfWork.CommitTransactionAsync();

        // 7. Map to ViewModel (include DepartmentName & Roles)
        var vm = _mapper.Map<UserDetailViewModel>(user);
        vm.Roles = (await _userManager.GetRolesAsync(user)).ToArray();

        return vm;
    }
}