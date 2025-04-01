namespace IMS.Domain.Entities;

public class Interview : BaseEntity
{
	public required string Title { get; set; }

	public InterviewStatus Status { get; set; } = InterviewStatus.New;

	public InterviewResult? Result { get; set; }

	public string? Location { get; set; }

	public string? MeetingID { get; set; }

	public string? Note { get; set; }

	public DateOnly InterviewDate { get; set; }

	public TimeOnly StartTime { get; set; }

	public TimeOnly EndTime { get; set; }

	#region Foreign Keys
	public int CreatedBy { get; set; }

	public int? CandidateId { get; set; }

	public int? JobId { get; set; }

	public int? RecruiterId { get; set; }
	#endregion

	#region Navigation Properties
	public User? UserCreated { get; set; }

	public Candidate? Candidate { get; set; }

	public Job? Job { get; set; }

	public ICollection<User>? Interviewers { get; set; }

	public User? Recruiter { get; set; }
	#endregion
}
