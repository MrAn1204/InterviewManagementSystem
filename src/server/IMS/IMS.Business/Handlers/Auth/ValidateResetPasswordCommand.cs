using System;
using MediatR;

namespace IMS.Business.Handlers;

public class ValidateResetPasswordCommand : IRequest<bool>
{
    public required string Token { get; set; }
}
