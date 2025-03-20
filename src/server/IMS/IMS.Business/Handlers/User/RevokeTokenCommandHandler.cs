using IMS.Business.Services;
using MediatR;

namespace IMS.Business.Handlers.User;

public class RevokeTokenCommandHandler(ITokenService tokenService) : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly ITokenService _tokenService = tokenService;

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
    }
}