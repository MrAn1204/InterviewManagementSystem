using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.Business.DTOs;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Handlers;
public class CheckUniqueQueryHandler : IRequestHandler<CheckUniqueQuery, CheckUniqueResultDto>
    {
        private readonly UserManager<User> _userManager;

        public CheckUniqueQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CheckUniqueResultDto> Handle(CheckUniqueQuery request, CancellationToken cancellationToken)
        {
            bool usernameExists = false;
            bool emailExists = false;

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var userByName = await _userManager.FindByNameAsync(request.Username);
                usernameExists = userByName != null;
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var userByEmail = await _userManager.FindByEmailAsync(request.Email);
                emailExists = userByEmail != null;
            }

            return new CheckUniqueResultDto
            {
                UsernameExists = usernameExists,
                EmailExists = emailExists
            };
        }
    }
