
using MediatR;

namespace IMS.Business.Handlers;

public class CreateMockUserCommand : IRequest<string>
{
    public required string Username { get; set; }
    public string Password { get; set; } = "Abc@123";
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public DateTime? DOB { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public required string[] Roles { get; set; } // Bỏ giá trị mặc định
    public string? Note { get; set; }
    public string? Gender { get; set; }
    public bool? IsActive { get; set; }
}

