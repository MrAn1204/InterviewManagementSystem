using IMS.Business.DTOs;
using MediatR;

namespace IMS.Business.Handlers;

public class RefreshTokenCommand : IRequest<LoginResultDto>
{
    public required string RefreshToken { get; set; }
}
