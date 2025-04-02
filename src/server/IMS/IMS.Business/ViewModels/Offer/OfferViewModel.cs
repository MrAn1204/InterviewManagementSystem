using System;

namespace IMS.Business.ViewModels;

public class OfferViewModel
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string? CandidateName { get; set; }
    public string? CandidateEmail { get; set; }
	public int? InterviewId { get; set; }
	public string? InterviewTitle { get; set; }

	public string Position { get; set; } = null!;
	public string ContractType { get; set; } = "FullTime"; // or use ENUM
	public DateTime ContractStart { get; set; }
	public DateTime? ContractEnd { get; set; }
	public string Status { get; set; } = "WaitingForApproval"; // or use ENUM
	public int ApprovedBy { get; set; }
	public string Approver { get; set; }

	public DateTime? ApprovedDate { get; set; }
	public decimal SalaryBasic { get; set; }
	public string? Note { get; set; }
	public DateTime? DueDate { get; set; }

    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }

}
