using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQuery, IEnumerable<UserByRoleViewModel>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWorks _unitOfWork;
    public GetUserByRoleQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userManager=userManager;
        _roleManager=roleManager;
    }

    public async Task<IEnumerable<UserByRoleViewModel>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
    {
        var users = new List<User>();

        foreach (var roleName in request.Roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                users.AddRange(usersInRole);
            }
        }

        return _mapper.Map<IEnumerable<UserByRoleViewModel>>(users.Distinct());
    }
}
