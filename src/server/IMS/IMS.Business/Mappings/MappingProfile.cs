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
  }
}