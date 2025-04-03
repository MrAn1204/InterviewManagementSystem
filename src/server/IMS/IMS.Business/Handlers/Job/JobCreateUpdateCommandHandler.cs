using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobCreateUpdateCommandHandler : BaseHandler,
    IRequestHandler<JobCreateUpdateCommand, JobViewModel>
{
    public JobCreateUpdateCommandHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public Task<JobViewModel> Handle(JobCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        return request.Id.HasValue
            ? Update(request, cancellationToken)
            : Create(request, cancellationToken);
    }

    private async Task<JobViewModel> Create(JobCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = new Job
        {
            Title = request.Title,
            WorkingAddress = request.WorkingAddress,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.UtcNow,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status ?? "Draft",
            JobLevels = request.Levels?.Select(levelId => new JobLevel
            {
                LevelId = levelId,
            }).ToList() ?? [],
            JobBenefits = request.Benefits?.Select(benefitId => new JobBenefit
            {
                BenefitId = benefitId,
            }).ToList() ?? [],
            JobSkills = request.Skills?.Select(skillId => new JobSkill
            {
                SkillId = skillId,
            }).ToList() ?? [],
        };

        _unitOfWork.JobRepository.Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Failed to create job");
        }

        var createdEntity = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.UserCreated)
            .Include(x => x.JobLevels)
                .ThenInclude(x => x.Level)
            .Include(x => x.JobBenefits)
                .ThenInclude(x => x.Benefit)
            .Include(x => x.JobSkills)
                .ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken) ??
            throw new ResourceNotFoundException($"Job with ID {entity.Id} not found");

        return _mapper.Map<JobViewModel>(createdEntity);
    }

    private async Task<JobViewModel> Update(JobCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.JobRepository.GetQuery()
                    .Include(x => x.JobLevels)
                    .Include(x => x.JobSkills)
                    .Include(x => x.JobBenefits)
                    .FirstOrDefaultAsync(x => x.Id == request.Id!.Value, cancellationToken) ??
                    throw new ResourceNotFoundException($"Job with {request.Id} is not found");
        entity.JobLevels.Clear();
        entity.JobSkills.Clear();
        entity.JobBenefits.Clear();

        _mapper.Map(request, entity);
        entity.UpdatedDate = DateTime.UtcNow;

        if (request.Levels != null)
        {
            foreach (var levelId in request.Levels)
            {
                entity.JobLevels.Add(new JobLevel { LevelId = levelId, JobId = entity.Id });
            }
        }

        if (request.Skills != null)
        {
            foreach (var skillId in request.Skills)
            {
                entity.JobSkills.Add(new JobSkill { SkillId = skillId, JobId = entity.Id });
            }
        }

        if (request.Benefits != null)
        {
            foreach (var benefitId in request.Benefits)
            {
                entity.JobBenefits.Add(new JobBenefit { BenefitId = benefitId, JobId = entity.Id });
            }
        }

        _unitOfWork.JobRepository.Update(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Update category failed");
        }

        var updatedEntity = await _unitOfWork.JobRepository.GetQuery()
                .Include(x => x.UserCreated)
                .Include(x => x.JobBenefits)
                    .ThenInclude(x => x.Benefit)
                .Include(x => x.JobLevels)
                    .ThenInclude(x => x.Level)
                .Include(x => x.JobSkills)
                    .ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken) ??
                throw new ResourceNotFoundException($"Job with ID {entity.Id} not found");

        return _mapper.Map<JobViewModel>(updatedEntity);
    }
}
