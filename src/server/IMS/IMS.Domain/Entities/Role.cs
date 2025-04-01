
using Microsoft.AspNetCore.Identity;

namespace IMS.Domain.Entities;

public class Role : IdentityRole<int>
{
	// Navigation
	public ICollection<UserRole>? UserRoles { get; set; }
}