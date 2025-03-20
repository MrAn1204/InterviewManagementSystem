using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Role
{
	public int Id { get; set; }
	public string RoleName { get; set; } = null!;

	// Navigation
	public ICollection<UserRole>? UserRoles { get; set; }
}