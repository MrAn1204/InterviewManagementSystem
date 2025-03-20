using IMS.Business.Services;
using MediatR;

namespace IMS.Business.Handlers.User;

public class ForgotPasswordCommandHandler(IEmailService emailService) : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // TODO: Check if email exists on database
        
        string subject = "Password Reset";
        string message = $@"
            <html>
            <body>
                <p>We have just received a password reset request for <b>{request.Email}</b>.</p>
                <p>Please click <a href='http://localhost:4200/reset-password' style='color: blue; text-decoration: underline;'>here</a> to reset your password.</p>
                <p>For your security, the link will expire in <b>24 hours</b> or immediately after you reset your password.</p>
                <br>
                <p>Thanks & Regards!<br>
                <b>IMS Team</b></p>
            </body>
            </html>
        ";

        await _emailService.SendEmailAsync(request.Email, subject, message);
    }
}
