using MediatR;

namespace IMS.Business.Handlers.User;

public class ResetPasswordCommandHandler() : IRequestHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!request.NewPassword.Equals(request.ConfirmNewPassword))
        {
            throw new ArgumentException("Passwords do not match");
        }

        // TODO: Update password on database

        return true;
    }
}