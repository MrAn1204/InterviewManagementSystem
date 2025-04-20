using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSearchQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewSearchQuery, PaginatedResult<InterviewViewModel>>
{
    public async Task<PaginatedResult<InterviewViewModel>> Handle(InterviewSearchQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.InterviewRepository.GetQuery();

        if (request.Keyword != null)
        {
            query = query.Where(interview => interview.Title.Contains(request.Keyword));
        }

        if (request.InterviewerId > 0)
        {
            query = query.Where(i => i.Interviewers!
                .Select(u => u.Id)
                .Contains(request.InterviewerId!.Value));
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            query = query.Where(interview => interview.Status.ToString() == request.Status);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query.Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize)
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .Include(interview => interview.Job)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<InterviewViewModel>>(items);

        return new PaginatedResult<InterviewViewModel>(request.PageNumber, request.PageSize, total, result);
    }
}
