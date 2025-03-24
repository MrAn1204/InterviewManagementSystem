using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class LogoutCommandHandler(ITokenService tokenService) : IRequestHandler<LogoutCommand, bool>
{
    private readonly ITokenService _tokenService = tokenService;

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
        
        return true;
    }
}
