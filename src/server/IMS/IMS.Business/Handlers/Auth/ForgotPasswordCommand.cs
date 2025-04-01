using MediatR;

namespace IMS.Business.Handlers;

public class ForgotPasswordCommand : IRequest
{
    public required string Email { get; set; }
}
