
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.ViewModels;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<UserDetailViewModel>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;

    public GetUserListQueryHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<UserDetailViewModel>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> query = _userManager.Users
            .Include(u => u.UserRoles)
            .Include(u => u.Department);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                u.UserName.Contains(request.Search) ||
                u.Email.Contains(request.Search) ||
                u.FullName.Contains(request.Search) ||
                (u.Address != null && u.Address.Contains(request.Search)));
        }

        var normalizedRoles = request.Roles?.Select(r => r.ToUpper()).ToList() ?? new List<string>();

        if (normalizedRoles.Any())
        {
            var roles = await _roleManager.Roles
                .Where(r => normalizedRoles.Contains(r.NormalizedName.ToUpper()))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            if (roles.Any())
            {
                query = query.Where(u => u.UserRoles.Any(ur => roles.Contains(ur.RoleId)));
            }
            else
            {
                return new PaginatedResult<UserDetailViewModel>(request.PageNumber, request.PageSize, 0, new UserDetailViewModel[0]);
            }
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(u => u.DepartmentId == request.DepartmentId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var userViewModels = new List<UserDetailViewModel>();
        foreach (var user in users)
        {
            var userVm = _mapper.Map<UserDetailViewModel>(user);
            var rolesForUser = await _userManager.GetRolesAsync(user);
            userVm.Roles = rolesForUser.ToArray();
            userViewModels.Add(userVm);
        }

        return new PaginatedResult<UserDetailViewModel>(
            request.PageNumber, 
            request.PageSize, 
            totalCount, 
            userViewModels.ToArray()
        );
    }

}


