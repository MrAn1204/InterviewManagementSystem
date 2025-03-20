using MediatR;

namespace IMS.Business.Handlers.User;

public class RevokeTokenCommand : IRequest<bool>
{
    public required string RefreshToken { get; set; }
}
