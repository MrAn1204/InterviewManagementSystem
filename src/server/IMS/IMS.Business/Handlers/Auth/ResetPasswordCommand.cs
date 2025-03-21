using MediatR;

namespace IMS.Business.Handlers;

public class ResetPasswordCommand : IRequest<bool>
{
    public required string Email { get; set; }

    public required string NewPassword { get; set; }

    public required string ConfirmNewPassword { get; set; }
}
