using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewGetByIdQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewGetByIdQuery, InterviewViewModel>
{
    public async Task<InterviewViewModel> Handle(InterviewGetByIdQuery request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .FirstOrDefaultAsync(interview => interview.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        return _mapper.Map<InterviewViewModel>(interview);
    }
}
