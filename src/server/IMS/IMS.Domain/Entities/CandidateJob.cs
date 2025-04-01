

namespace IMS.Domain.Entities;

public class CandidateJob
{
	public int CandidateId { get; set; }
	public int JobId { get; set; }

	public DateTime? AppliedDate { get; set; }

	public Candidate? Candidate { get; set; }
	public Job? Job { get; set; }
}