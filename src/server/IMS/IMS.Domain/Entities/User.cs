using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class User : BaseEntity
{
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string FullName { get; set; } = null!;
	public string? PhoneNumber { get; set; }
	public string? Address { get; set; }
	public string? Note { get; set; }
	public DateTime? DOB { get; set; }
	public bool IsActive { get; set; } = true;

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
}