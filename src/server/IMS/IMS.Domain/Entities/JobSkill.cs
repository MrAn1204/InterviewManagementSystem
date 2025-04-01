

namespace IMS.Domain.Entities;

public class JobSkill
{
	public int JobId { get; set; }
	public int SkillId { get; set; }

	public string? Note { get; set; }

	public Job? Job { get; set; }
	public Skill? Skill { get; set; }
}