using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobGetAllQueryHandler : BaseHandler, 
    IRequestHandler<JobGetAllQuery, IEnumerable<JobViewModel>>
{
    public JobGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<JobViewModel>> Handle(JobGetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.JobRepository.GetQuery().Include(x => x.UserCreated)
            .Include(x => x.JobBenefits)
                .ThenInclude(jb => jb.Benefit)
            .Include(x => x.JobLevels)
                .ThenInclude(jl => jl.Level)
            .Include(x => x.JobSkills)
                .ThenInclude(js => js.Skill)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<JobViewModel>>(result);
    }
}
