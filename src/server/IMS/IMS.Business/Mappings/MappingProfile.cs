using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Business.ViewModels.Skill;
using IMS.Business.ViewModels;
using IMS.Domain.Entities;
using IMS.Business.ViewModels.Benefit;

namespace IMS.Business.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<SkillViewModel, Skill>().ReverseMap();
    CreateMap<LevelViewModel, Level>().ReverseMap();
    CreateMap<BenefitViewModel, Benefit>().ReverseMap();
    CreateMap<UserByRoleViewModel, User>().ReverseMap();
    CreateMap<Candidate, CandidateViewModel>()
            .ForPath(dest => dest.Recruiter.Id, opt => opt.MapFrom(src => src.Recruiter.Id))
            .ForPath(dest => dest.Recruiter.FullName, opt => opt.MapFrom(src => src.Recruiter.FullName))
            .ForPath(dest => dest.Recruiter.UserName, opt => opt.MapFrom(src => src.Recruiter.UserName))
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel { Id = cs.SkillId, SkillName = cs.Skill.SkillName })));
   
    CreateMap<JobViewModel, Job>().ReverseMap()
    .ForMember(dest => dest.UserCreatedName, opt => opt.MapFrom(src =>
        src.UserCreated != null ? src.UserCreated.FullName : ""))
    .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src =>
        src.JobBenefits != null
        ? src.JobBenefits.Select(jb => new BenefitViewModel
        {
          Id = jb.BenefitId,
          BenefitName = jb.Benefit != null ? jb.Benefit.BenefitName : "",
          Description = jb.Benefit != null ? jb.Benefit.Description : ""
        }).ToList()
        : new List<BenefitViewModel>()
    ))
    .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
        src.JobSkills != null
        ? src.JobSkills.Select(js => new SkillViewModel
        {
          Id = js.SkillId,
          SkillName = js.Skill != null ? js.Skill.SkillName : ""
        }).ToList()
        : new List<SkillViewModel>()
    ))
    .ForMember(dest => dest.Levels, opt => opt.MapFrom(src =>
        src.JobLevels != null
        ? src.JobLevels.Select(jl => new LevelViewModel
        {
          Id = jl.LevelId,
          LevelName = jl.Level != null ? jl.Level.LevelName : "",
          Description = jl.Level != null ? jl.Level.Description : ""
        }).ToList()
        : new List<LevelViewModel>()
    ));

  }
}