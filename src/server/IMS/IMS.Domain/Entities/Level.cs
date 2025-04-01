

namespace IMS.Domain.Entities;

public class Level
{
	public int Id { get; set; }

	public string LevelName { get; set; } = null!;
	public string? Description { get; set; }

	// N:N -> Job
	public ICollection<JobLevel>? JobLevels { get; set; }
}