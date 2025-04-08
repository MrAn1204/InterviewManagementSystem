
namespace IMS.Domain.Entities;

public class Offer : BaseEntity
{
	public int CandidateId { get; set; }
	public int? LevelId { get; set; }
	public int? InterviewId { get; set; } // 1 Interview -> N Offer
	public int? DepartmentId { get; set; } // 1 Department -> N Offer

	public string Position { get; set; } = null!;
	public string ContractType { get; set; } = "Trial 2 months"; // or use ENUM
	public DateTime ContractStart { get; set; }
	public DateTime? ContractEnd { get; set; }
	public string Status { get; set; } = "WaitingForApproval"; // or use ENUM
	public int ApprovedBy { get; set; }

	public DateTime? ApprovedDate { get; set; }
	public decimal SalaryBasic { get; set; }
	public string? Note { get; set; }
	public DateTime? DueDate { get; set; }

	// Navigation
	public Candidate? Candidate { get; set; }
	public Department? Department { get; set; }

	public Level? Level { get; set; }
	public Interview? Interview { get; set; } // 1:N => Offer
	public User? UserApproved { get; set; }

	// N:N -> Department (OfferDepartment)
	// public ICollection<OfferDepartment>? OfferDepartments { get; set; }
}