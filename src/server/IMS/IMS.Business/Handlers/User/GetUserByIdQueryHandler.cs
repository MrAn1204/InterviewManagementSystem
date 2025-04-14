using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers
{
    public class GetUserByIdQueryHandler(
    UserManager<User> userManager,
    IUnitOfWorks unitOfWork, IMapper _mapper) : IRequestHandler<GetUserByIdQuery, UserDetailViewModel>
{
    public async Task<UserDetailViewModel> Handle(
        GetUserByIdQuery request, 
        CancellationToken cancellationToken)
    {
            // Get user with department
            var user = await unitOfWork.UserRepository
            .GetAllQuery()
            .Include(u => u.Department) 
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
                new ResourceNotFoundException($"User with ID {request.UserId} not found");

        // Map và trả về kết quả
        var result = _mapper.Map<UserDetailViewModel>(user);
    
        result.Roles = (await userManager.GetRolesAsync(user)).ToArray();
        
        return result;
        }
    }
}