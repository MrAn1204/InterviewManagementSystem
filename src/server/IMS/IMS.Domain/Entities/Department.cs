using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Department
{
	public int Id { get; set; }
	public string DepartmentName { get; set; } = null!;

	// 1:N -> User
	public ICollection<User>? Users { get; set; }

	// 1:N -> Offer
	public ICollection<Offer>? Offers { get; set; }
}