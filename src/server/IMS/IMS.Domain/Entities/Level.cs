

namespace IMS.Domain.Entities;

public class Level : BaseEntity
{
	public string LevelName { get; set; } = null!;
	public string? Description { get; set; }

	// N:N -> Job
	public ICollection<JobLevel>? JobLevels { get; set; }
	public ICollection<Offer>? Offers { get; set; }
}