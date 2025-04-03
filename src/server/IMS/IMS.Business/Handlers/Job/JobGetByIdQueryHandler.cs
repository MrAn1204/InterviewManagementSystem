using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobGetByIdQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<JobGetByIdQuery, JobViewModel>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;
    public async Task<JobViewModel> Handle(JobGetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.UserCreated)
            .Include(x => x.JobLevels)
                .ThenInclude(x => x.Level)
            .Include(x => x.JobBenefits)
                .ThenInclude(x => x.Benefit)
            .Include(x => x.JobSkills)
                .ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
            throw new ResourceNotFoundException($"Job with ID {request.Id} not found");

        return _mapper.Map<JobViewModel>(result);
    }
}
