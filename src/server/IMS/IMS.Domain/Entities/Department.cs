

namespace IMS.Domain.Entities;

public class Department
{
	public int Id { get; set; }
	public string DepartmentName { get; set; } = null!;

	// 1:N -> User
	public ICollection<User>? Users { get; set; }

	// N:N -> Offer (OfferDepartment) (nếu vẫn cần)
	public ICollection<OfferDepartment>? OfferDepartments { get; set; }
}