using System.ComponentModel.DataAnnotations;

namespace IMS.Domain.Entities;

public class Interview : BaseEntity
{
	[Required]
	[MaxLength(100)]
	public required string Title { get; set; }

	public InterviewStatus Status { get; set; } = InterviewStatus.New;

	public InterviewResult? Result { get; set; }

	public string? Location { get; set; }

	public string? MeetingID { get; set; }

	[MaxLength(500)]
	public string? Note { get; set; }

	[Required]
	public DateOnly InterviewDate { get; set; }

	[Required]
	public TimeOnly StartTime { get; set; }

	[Required]
	public TimeOnly EndTime { get; set; }

	#region Foreign Keys
	public int CreatedBy { get; set; }

	public int? CandidateId { get; set; }

	public int? JobId { get; set; }

	public int? RecruiterId { get; set; }
	#endregion

	#region Navigation Properties
	public User? UserCreated { get; set; }

	public virtual Candidate? Candidate { get; set; }

	public virtual Job? Job { get; set; }

	public virtual ICollection<User>? Interviewers { get; set; }

	public virtual User? Recruiter { get; set; }
	#endregion
}
