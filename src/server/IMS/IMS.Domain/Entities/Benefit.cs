namespace IMS.Domain.Entities
{
	public class Benefit
	{
		public int Id { get; set; }

		public string BenefitName { get; set; } = null!;
		public string? Description { get; set; }

		// N:N -> Job
		public ICollection<JobBenefit>? JobBenefits { get; set; }
	}
}