using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;

namespace IMS.Business.Handlers;

public class CandidateCountByWeekQueryHandler(IUnitOfWorks unitOfWorks, IMapper mapper) : BaseHandler(unitOfWorks, mapper), IRequestHandler<CandidateCountByWeekQuery, IEnumerable<CandidateCountByWeekViewModel>>
{

    public async Task<IEnumerable<CandidateCountByWeekViewModel>> Handle(CandidateCountByWeekQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.CandidateRepository.GetQuery();
        var candidates = query.Where(c => c.CreatedDate.Year == request.Year && c.CreatedDate.Month == request.Month);
        var result = new List<CandidateCountByWeekViewModel>
        {
            new CandidateCountByWeekViewModel { WeekNumber = 1, CandidateCount=candidates.Count(c => c.CreatedDate.Day <= 7) },
            new CandidateCountByWeekViewModel { WeekNumber = 1, CandidateCount=candidates.Count(c => c.CreatedDate.Day > 7 && c.CreatedDate.Day <= 14) },
            new CandidateCountByWeekViewModel { WeekNumber = 1, CandidateCount=candidates.Count(c => c.CreatedDate.Day > 14 && c.CreatedDate.Day <= 21)  },
            new CandidateCountByWeekViewModel { WeekNumber = 1, CandidateCount=candidates.Count(c => c.CreatedDate.Day > 21) }
        };
        return result;
    }
}
