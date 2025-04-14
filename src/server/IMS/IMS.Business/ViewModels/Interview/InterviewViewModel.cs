using System.ComponentModel.DataAnnotations;
using IMS.Core.ViewModels;
using IMS.Domain;

namespace IMS.Business.ViewModels;

public class InterviewViewModel : BaseViewModel
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Required]
    public required DateOnly InterviewDate { get; set; }

    [Required]
    public required TimeOnly StartTime { get; set; }

    [Required]
    public required TimeOnly EndTime { get; set; }

    [MaxLength(500)]
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

    public string? CreatedDate { get; set; }

    public string? UpdatedDate { get; set; }
}
