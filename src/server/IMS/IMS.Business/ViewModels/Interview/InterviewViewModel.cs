using IMS.Core.ViewModels;
using IMS.Domain;

namespace IMS.Business.ViewModels;

public class InterviewViewModel : BaseViewModel
{
    public required string Title { get; set; }

    public required DateOnly InterviewDate { get; set; }

    public required TimeOnly StartTime { get; set; }

    public required TimeOnly EndTime { get; set; }

    public string? Note { get; set; }

    public string? Location { get; set; }

    public string? MeetingId { get; set; }

    public string? Status { get; set; } = InterviewStatus.New.ToString();

    public string? Result { get; set; } = "N/A";

    public int? CandidateId { get; set; }
    
    public string? CandidateName { get; set; }
    
    public int? JobId { get; set; }
    
    public string? JobName { get; set; }
    
    public ICollection<int>? InterviewersId { get; set; }
    
    public ICollection<string>? InterviewersName { get; set; }
    
    public int? RecruiterId { get; set; }
    
    public string? RecruiterName { get; set; }
}
