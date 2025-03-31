using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Business.ViewModels.Skill;
using IMS.Business.ViewModels;
using IMS.Domain.Entities;

namespace IMS.Business.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<SkillViewModel, Skill>().ReverseMap();
    CreateMap<LevelViewModel, Level>().ReverseMap();
    CreateMap<UserByRoleViewModel, User>().ReverseMap();
    CreateMap<Candidate, CandidateViewModel>()
            .ForPath(dest => dest.Recruiter.Id, opt => opt.MapFrom(src => src.Recruiter.Id))
            .ForPath(dest => dest.Recruiter.FullName, opt => opt.MapFrom(src => src.Recruiter.FullName))
            .ForPath(dest => dest.Recruiter.UserName, opt => opt.MapFrom(src => src.Recruiter.UserName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == null ? -1 : (src.Gender == true ? 1 : 0)))
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel { Id = cs.SkillId, SkillName = cs.Skill.SkillName })))
            .ForMember(dest => dest.HighestLevel, opt => opt.MapFrom(src => new LevelViewModel { Id = src.HighestLevel.Id, LevelName = src.HighestLevel.LevelName }));
  }
}