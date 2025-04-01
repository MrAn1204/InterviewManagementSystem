using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Business.ViewModels.Skill;
using IMS.Business.ViewModels;
using IMS.Domain.Entities;
using IMS.Business.ViewModels.Offer;

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
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel { Id = cs.SkillId, SkillName = cs.Skill.SkillName })));
    
    CreateMap<Offer, OfferViewModel>()
            .ForMember(dest => dest.DepartmentNames, opt => opt.MapFrom(src =>
                src.OfferDepartments.Select(od => od.Department.DepartmentName)));

  }
}