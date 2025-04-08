using System;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferCreateCommand : BaseCreateCommand<int>
{
    public int CandidateId { get; set; }
	public int? LevelId { get; set; }
	public int? InterviewId { get; set; }
	public int? DepartmentId { get; set; }

	public string Position { get; set; } = null!;
	public string ContractType { get; set; } = "Trial 2 months";
	public DateTime ContractStart { get; set; }
	public DateTime? ContractEnd { get; set; }
	public string Status { get; set; } = "WaitingForApproval";
	public int ApprovedBy { get; set; }

	public DateTime? ApprovedDate { get; set; }
	public decimal SalaryBasic { get; set; }
	public string? Note { get; set; }
	public DateTime? DueDate { get; set; }
}
