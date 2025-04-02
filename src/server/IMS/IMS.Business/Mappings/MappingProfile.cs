using AutoMapper;
using IMS.Business.Handlers;
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
    CreateMap<BenefitViewModel, Benefit>().ReverseMap();
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
    CreateMap<Candidate, CandidateViewModel>()
            .ForPath(dest => dest.Recruiter.Id, opt => opt.MapFrom(src => src.Recruiter.Id))
            .ForPath(dest => dest.Recruiter.FullName, opt => opt.MapFrom(src => src.Recruiter.FullName))
            .ForPath(dest => dest.Recruiter.UserName, opt => opt.MapFrom(src => src.Recruiter.UserName))
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel { Id = cs.SkillId, SkillName = cs.Skill.SkillName })));

    CreateMap<JobViewModel, Job>().ReverseMap()
    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
    .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
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

    CreateMap<JobCreateUpdateCommand, Job>().ReverseMap()
      .ForMember(dest => dest.Levels, opt => opt.MapFrom(src =>
        src.JobLevels != null
        ? src.JobLevels.Select(x => new LevelViewModel
        {
          Id = x.LevelId,
          LevelName = x.Level != null ? x.Level.LevelName : "",
          Description = x.Level != null ? x.Level.Description : ""
        }).ToList()
        : new List<LevelViewModel>()))
      .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src =>
        src.JobBenefits != null
        ? src.JobBenefits.Select(x => new BenefitViewModel
        {
          Id = x.BenefitId,
          BenefitName = x.Benefit != null ? x.Benefit.BenefitName : "",
          Description = x.Benefit != null ? x.Benefit.Description : ""
        }).ToList()
        : new List<BenefitViewModel>()))
      .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
        src.JobSkills != null
        ? src.JobSkills.Select(x => new SkillViewModel
        {
          Id = x.SkillId,
          SkillName = x.Skill != null ? x.Skill.SkillName : ""
        }).ToList()
        : new List<SkillViewModel>())
      );
  }
}