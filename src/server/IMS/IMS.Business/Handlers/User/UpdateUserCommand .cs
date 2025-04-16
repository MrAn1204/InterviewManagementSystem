using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace IMS.Business.Handlers
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
        public string? Note { get; set; }

        public string? Gender { get; set; } 
}
}