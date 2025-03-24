using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class Skill
{
	public int Id { get; set; }

	public string SkillName { get; set; } = null!;

	// N:N -> Candidate
	public ICollection<CandidateSkill>? CandidateSkills { get; set; }

	// N:N -> Job (JobSkill)
	public ICollection<JobSkill>? JobSkills { get; set; }
}