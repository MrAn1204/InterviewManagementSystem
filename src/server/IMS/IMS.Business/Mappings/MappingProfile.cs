using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Business.ViewModels.Skill;
using IMS.Business.ViewModels;
using IMS.Domain.Entities;
using IMS.Business.Handlers;

namespace IMS.Business.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<SkillViewModel, Skill>().ReverseMap();
    CreateMap<LevelViewModel, Level>().ReverseMap();
    CreateMap<UserByRoleViewModel, User>().ReverseMap();
        _ = CreateMap<Candidate, CandidateViewModel>()
                .ForPath(dest => dest.Recruiter.Id, opt => opt.MapFrom(src => src.Recruiter.Id))
                .ForPath(dest => dest.Recruiter.FullName, opt => opt.MapFrom(src => src.Recruiter.FullName))
                .ForPath(dest => dest.Recruiter.UserName, opt => opt.MapFrom(src => src.Recruiter.UserName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == null ? -1 : (src.Gender == true ? 1 : 0)))
                .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel { Id = cs.SkillId, SkillName = cs.Skill.SkillName })))
                .ForMember(dest => dest.HighestLevel, opt => opt.MapFrom(src => new LevelViewModel { Id = src.HighestLevel.Id, LevelName = src.HighestLevel.LevelName }));
    
    CreateMap<Offer, OfferViewModel>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));



    CreateMap<Interview, InterviewViewModel>()
      .ForMember(dest => dest.CandidateId, opt => opt.MapFrom<int?>(
        src => src.Candidate != null ? src.Candidate.Id : null))
      .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(
        src => src.Candidate != null ? src.Candidate.FullName : null))
      .ForMember(dest => dest.RecruiterId, opt => opt.MapFrom<int?>(
        src => src.Recruiter != null ? src.Recruiter.Id : null))
      .ForMember(dest => dest.RecruiterName, opt => opt.MapFrom(
        src => src.Recruiter != null ? src.Recruiter.FullName : null))
      .ForMember(dest => dest.JobId, opt => opt.MapFrom<int?>(
        src => src.Job != null ? src.Job.Id : null))
      .ForMember(dest => dest.JobName, opt => opt.MapFrom(
        src => src.Job != null ? src.Job.Title : null))
      .ForMember(dest => dest.InterviewersId, opt => opt.MapFrom(
        src => src.Interviewers != null ? src.Interviewers.Select(i => i.Id) : null))
      .ForMember(dest => dest.InterviewersName, opt => opt.MapFrom(
        src => src.Interviewers != null ? src.Interviewers.Select(i => i.FullName) : null))
      .ReverseMap();

    CreateMap<InterviewCreateUpdateCommand, Interview>()
      .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.CandidateId))
      .ForMember(dest => dest.RecruiterId, opt => opt.MapFrom(src => src.RecruiterId))
      .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.JobId))
      .ForMember(dest => dest.Interviewers, opt => opt.MapFrom((src, _, _, context) =>
        src.InterviewersId != null
          ? [.. src.InterviewersId
              .Select(id => context.Items["Users"] is ICollection<User> users 
                ? users.FirstOrDefault(u => u.Id == id) : null)
              .Where(u => u != null)]
          : new List<User>()))
      .ReverseMap();
  }
}