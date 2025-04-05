using IMS.Business.Services;
using MediatR;

namespace IMS.Business.Handlers;

public class ValidateResetPasswordCommandHandler(
    ITokenService tokenService
) : IRequestHandler<ValidateResetPasswordCommand, bool>
{
    private readonly ITokenService _tokenService = tokenService;

    public async Task<bool> Handle(ValidateResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _tokenService.ValidateResetPasswordAsync(request.Token);
        
        return result;
    }
}