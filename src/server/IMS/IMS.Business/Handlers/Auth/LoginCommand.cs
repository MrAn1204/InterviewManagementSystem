using System;
using IMS.Business.DTOs;
using MediatR;

namespace IMS.Business.Handlers;

public class LoginCommand : IRequest<LoginResultDto>
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}

