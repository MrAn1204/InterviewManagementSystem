using System;

namespace IMS.Business.ViewModels.Offer;

public class OfferViewModel
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
	public int? InterviewId { get; set; } // 1 Interview -> N Offer

	public string Position { get; set; } = null!;
	public string ContractType { get; set; } = "FullTime"; // or use ENUM
	public DateTime ContractStart { get; set; }
	public DateTime? ContractEnd { get; set; }
	public string Status { get; set; } = "WaitingForApproval"; // or use ENUM
	public int ApprovedBy { get; set; }

	public DateTime? ApprovedDate { get; set; }
	public decimal SalaryBasic { get; set; }
	public string? Note { get; set; }
	public DateTime? DueDate { get; set; }

    public string? DepartmentName { get; set; }

    // public List<string> DepartmentNames { get; set; } = new List<string>();
}
