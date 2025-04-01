
using MediatR;

namespace IMS.Business.Handlers;

public class CreateMockUserCommand : IRequest<string>
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string FullName { get; set; }

    public int DepartmentId { get; set; } = 1;

    public string[] Roles { get; set; } = ["Admin", "Manager", "Recruiter", "Interviewer"];
}

