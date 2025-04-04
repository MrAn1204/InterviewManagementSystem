

namespace IMS.Domain.Entities;

public class Department : BaseEntity
{
	public string DepartmentName { get; set; } = null!;

	// 1:N -> User
	public ICollection<User>? Users { get; set; }

	// 1:N -> Offer
	public ICollection<Offer>? Offers { get; set; }
}