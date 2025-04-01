using IMS.Business.ViewModels.Level;
using IMS.Business.ViewModels.Skill;

namespace IMS.Business.ViewModels;

public class CandidateViewModel
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? CurrentPosition { get; set; }
    public string? Note { get; set; }
    public int Experience { get; set; }
    public string? CV { get; set; }
    public string? Status { get; set; }
    public ICollection<SkillViewModel>? CandidateSkills { get; set; }
    public UserByRoleViewModel? Recruiter { get; set; }
    public LevelViewModel? HighestLevel { get; set; }
}
