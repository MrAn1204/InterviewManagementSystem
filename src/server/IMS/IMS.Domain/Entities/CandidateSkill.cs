using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Entities;

public class CandidateSkill
{
	public int CandidateId { get; set; }
	public int SkillId { get; set; }

	public int? ExperienceYear { get; set; }

	public Candidate? Candidate { get; set; }
	public Skill? Skill { get; set; }
}