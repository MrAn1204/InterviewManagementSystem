using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMS.Business.DTOs;
using IMS.Business.Services;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Business.Handlers;

public class CreateMockUserCommandHandler(
    UserManager<User> userManager
) : IRequestHandler<CreateMockUserCommand, string>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> Handle(CreateMockUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            DepartmentId = request.DepartmentId
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (createResult.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, request.Roles);
        }
        else
        {
            return createResult.Errors.First().Description;
        }

        return string.Empty;
    }
}
