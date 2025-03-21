using MediatR;

namespace IMS.Business.Handlers;

public class LogoutCommand : IRequest<bool>
{
    public required string RefreshToken { get; set; }
}

