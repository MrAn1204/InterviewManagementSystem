using IMS.Business.Services;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class ForgotPasswordCommandHandler(
    IEmailService emailService,
    ITokenService tokenService,
    UserManager<User> userManager
) : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IEmailService _emailService = emailService;

    private readonly ITokenService _tokenService = tokenService;

    private readonly UserManager<User> _userManager = userManager;

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new ArgumentException("This email is not linked to any account.");
        }

        var token = await _tokenService.GenerateResetPasswordTokenAsync(user.Id);
        
        var resetLink = $"http://localhost:4200/reset-password?token={token.Token}";
        string subject = "Password Reset";
        string message = $@"
            <html>
            <body>
                <p>We have just received a password reset request for <b>{request.Email}</b>.</p>
                <p>Please click <a href='{resetLink}' style='color: blue; text-decoration: underline;'>here</a> to reset your password.</p>
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
