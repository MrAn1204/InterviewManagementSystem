using Microsoft.AspNetCore.Http;

namespace IMS.Business.Handlers;

public class CandidateCreateCommand : BaseCreateCommand<int>
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? DOB { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public int? Gender { get; set; }
    public IFormFile? CvAttachment { get; set; }
    public string? Note { get; set; }
    public string? Position { get; set; }
    public string? Status { get; set; }
    public List<int>? Skills { get; set; }
    public int YearOfExperience { get; set; }
    public int Recruiter { get; set; }
    public int HighestLevel { get; set; }

}
