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

    public string Status { get; set; } = InterviewStatus.New.ToString();

    public string Result { get; set; } = "N/A";

    public required int CandidateId { get; set; }
    
    public required string CandidateName { get; set; }
    
    public required int JobId { get; set; }
    
    public required string JobName { get; set; }
    
    public required ICollection<int> InterviewersId { get; set; }
    
    public required ICollection<string> InterviewersName { get; set; }
    
    public required int RecruiterId { get; set; }
    
    public string? RecruiterName { get; set; }
}
