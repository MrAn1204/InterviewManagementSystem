using IMS.Business.ViewModels;

namespace IMS.Business.Handlers;

public class JobCreateUpdateCommand : BaseCreateUpdateCommand<JobViewModel>
{
    public string Title { get; set; } = null!;
	public string WorkingAddress { get; set; } = null!;
	public decimal SalaryMin { get; set; }
	public decimal SalaryMax { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public string? Status { get; set; } = "Draft"; 
	public int CreatedBy { get; set; }

	public List<int> Levels { get; set; } = [];
	public List<int> Benefits { get; set; } = [];
	public List<int> Skills { get; set; } = [];
}
