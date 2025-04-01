using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Job : BaseEntity
{
	public string Title { get; set; } = null!;
	public string WorkingAddress { get; set; } = null!;
	public decimal SalaryMin { get; set; }
	public decimal SalaryMax { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public string Status { get; set; } = "Draft"; // or use ENUM

	public int CreatedBy { get; set; } // FK -> User
	public User? UserCreated { get; set; }

	// N:N -> Candidate (CandidateJob)
	public ICollection<CandidateJob>? CandidateJobs { get; set; }

	// N:N -> Benefit (JobBenefit)
	public ICollection<JobBenefit>? JobBenefits { get; set; }

	// N:N -> Skill (JobSkill)
	public ICollection<JobSkill>? JobSkills { get; set; }

	// N:N -> Level (JobLevel)
	public ICollection<JobLevel>? JobLevels { get; set; }

	// 1:N -> Offer
	// public ICollection<Offer>? Offers { get; set; }

	// 1:N -> Interview
	public ICollection<Interview>? Interviews { get; set; }
}