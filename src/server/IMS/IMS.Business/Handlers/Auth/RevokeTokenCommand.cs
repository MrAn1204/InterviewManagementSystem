using MediatR;

namespace IMS.Business.Handlers;

public class RevokeTokenCommand : IRequest<bool>
{
    public required string RefreshToken { get; set; }
}
