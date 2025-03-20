using MediatR;

namespace IMS.Business.Handlers.User;

public class ResetPasswordCommand : IRequest<bool>
{
    public required string NewPassword { get; set; }

    public required string ConfirmNewPassword { get; set; }
}
