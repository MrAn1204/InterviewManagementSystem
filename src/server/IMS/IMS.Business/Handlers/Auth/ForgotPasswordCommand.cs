using System;
using MediatR;

namespace IMS.Business.Handlers.User;

public class ForgotPasswordCommand : IRequest
{
    public required string Email { get; set; }
}
