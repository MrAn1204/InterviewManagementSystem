namespace IMS.Business.ViewModels;

public class JobViewModel
{
    
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string WorkingAddress { get; set; } = null!;
    
    public decimal SalaryMin { get; set; }
    
    public decimal SalaryMax { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string Status { get; set; } = "Draft";
    
    public int CreatedBy { get; set; } 
    
    public string? UserCreatedName { get; set; }
    
    public ICollection<BenefitViewModel>? Benefits { get; set; }

	public ICollection<SkillViewModel>? Skills { get; set; }

	public ICollection<LevelViewModel>? Levels { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
