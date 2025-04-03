
using Microsoft.AspNetCore.Identity;

namespace IMS.Domain.Entities;

public class User : IdentityUser<int>, IBaseEntity
{
	public string FullName { get; set; } = null!;
	public string? Address { get; set; }
	public string? Note { get; set; }
	public string? Gender { get; set; }
	public DateTime? DOB { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedDate { get; set; }

	// 1:N -> Department
	public int DepartmentId { get; set; }

	public Department Department { get; set; }

	// N:N -> Role (UserRole)
	public ICollection<UserRole>? UserRoles { get; set; }

	// 1:N -> Job (createdBy)
	public ICollection<Job>? JobsCreated { get; set; }

	// 1:N -> Offer (approvedBy)
	public ICollection<Offer>? OffersApproved { get; set; }

	// 1:N -> Interview (createdBy)
	public ICollection<Interview>? InterviewsCreated { get; set; }
    public bool IsDelete { get ; set ; }
}