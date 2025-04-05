namespace IMS.Domain.Entities
{
	public class Benefit : BaseEntity
	{
		public string BenefitName { get; set; } = null!;
		public string? Description { get; set; }

		// N:N -> Job
		public ICollection<JobBenefit>? JobBenefits { get; set; }
	}
}