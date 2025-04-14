using IMS.Business.DTOs;
using MediatR;

namespace IMS.Business.Handlers;

    public class CheckUniqueQuery : IRequest<CheckUniqueResultDto>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }

