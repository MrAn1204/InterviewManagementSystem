using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class JobCountByWeekQueryHandler(IUnitOfWorks unitOfWorks, IMapper mapper) : BaseHandler(unitOfWorks, mapper), IRequestHandler<JobCountByWeekQuery, IEnumerable<JobCountByWeekViewModel>>
{

    public async Task<IEnumerable<JobCountByWeekViewModel>> Handle(JobCountByWeekQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.JobRepository.GetQuery();
        var Jobs = query.Where(c => c.CreatedDate.Year == request.Year && c.CreatedDate.Month == request.Month);
        var result = new List<JobCountByWeekViewModel>
        {
            new JobCountByWeekViewModel { WeekNumber = 1, JobCount=Jobs.Count(c => c.CreatedDate.Day <= 7) },
            new JobCountByWeekViewModel { WeekNumber = 1, JobCount=Jobs.Count(c => c.CreatedDate.Day > 7 && c.CreatedDate.Day <= 14) },
            new JobCountByWeekViewModel { WeekNumber = 1, JobCount=Jobs.Count(c => c.CreatedDate.Day > 14 && c.CreatedDate.Day <= 21)  },
            new JobCountByWeekViewModel { WeekNumber = 1, JobCount=Jobs.Count(c => c.CreatedDate.Day > 21) }
        };
        return result;
    }
}
