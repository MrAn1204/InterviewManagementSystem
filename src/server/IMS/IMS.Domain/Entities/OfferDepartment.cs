

namespace IMS.Domain.Entities;

public class OfferDepartment
{
	public int OfferId { get; set; }
	public int DepartmentId { get; set; }

	public Offer? Offer { get; set; }
	public Department? Department { get; set; }
}