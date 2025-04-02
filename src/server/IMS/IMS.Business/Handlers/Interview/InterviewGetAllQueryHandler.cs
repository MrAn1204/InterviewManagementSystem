using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewGetAllQuery, IEnumerable<InterviewViewModel>>
{
    public async Task<IEnumerable<InterviewViewModel>> Handle(InterviewGetAllQuery request, CancellationToken cancellationToken)
    {
        var interviews = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .Include(interview => interview.Job)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<InterviewViewModel>>(interviews);
    }
}
