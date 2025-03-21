using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IMS.Domain.Entities;

public class Role : IdentityRole<int>
{
	public string RoleName { get; set; } = null!;

	// Navigation
	public ICollection<UserRole>? UserRoles { get; set; }
}