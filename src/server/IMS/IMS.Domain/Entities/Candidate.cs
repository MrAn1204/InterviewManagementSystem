using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Candidate : BaseEntity
{
	public string FullName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
	public string Address { get; set; } = null!;
	public bool Gender { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? CurrentPosition { get; set; }
	public string? Note { get; set; }
	public int Experience { get; set; }
	public string Status { get; set; } = "Open"; // or use ENUM
	public string? CV { get; set; }

	// N:N -> Skill
	public ICollection<CandidateSkill>? CandidateSkills { get; set; }

	// N:N -> Job (CandidateJob)
	public ICollection<CandidateJob>? CandidateJobs { get; set; }

	// 1:N -> Offer (một candidate có nhiều offer)
	public ICollection<Offer>? Offers { get; set; }
}