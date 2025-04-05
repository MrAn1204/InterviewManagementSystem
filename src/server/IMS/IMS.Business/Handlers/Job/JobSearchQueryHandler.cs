using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class JobSearchQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : IRequestHandler<JobSearchQuery, PaginatedResult<JobViewModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<JobViewModel>> Handle(JobSearchQuery request, CancellationToken cancellationToken)
    {

        var query = _unitOfWork.JobRepository.GetQuery();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{request.Keyword}%") ||
                x.JobLevels.Any(jl => EF.Functions.Like(jl.Level!.LevelName, $"%{request.Keyword}%")) ||
                x.JobSkills.Any(js => EF.Functions.Like(js.Skill!.SkillName, $"%{request.Keyword}%"))
            );
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(x => x.Status == request.Status);
        }

        int total = await query.CountAsync(cancellationToken);

        query = query.OrderBy(x => x.Status == "Closed")
                        .ThenBy(x => x.Status == "Draft")
                        .ThenBy(x => x.Status == "Open")
               .ThenByDescending(x => x.CreatedDate);

        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .Include(c => c.JobLevels)
                .ThenInclude(c => c.Level)
            .Include(c => c.JobBenefits)
                .ThenInclude(c => c.Benefit)
            .Include(c => c.JobSkills)
                .ThenInclude(c => c.Skill)
            .ToListAsync(cancellationToken);

        var viewModels = _mapper.Map<IEnumerable<JobViewModel>>(items);

        return new PaginatedResult<JobViewModel>(request.PageNumber, request.PageSize, total, viewModels);
    }
}
