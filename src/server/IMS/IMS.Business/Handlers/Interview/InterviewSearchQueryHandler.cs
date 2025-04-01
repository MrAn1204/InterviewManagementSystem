using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Core.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSearchQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper, UserManager<User> userManager)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewSearchQuery, PaginatedResult<InterviewViewModel>>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<PaginatedResult<InterviewViewModel>> Handle(InterviewSearchQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.InterviewRepository.GetQuery();

        if (request.Keyword != null)
        {
            query = query.Where(interview => interview.Title.Contains(request.Keyword));
        }

        if (request.InterviewerId != null)
        {
            var interviewer = await _userManager.FindByIdAsync(request.InterviewerId.ToString()!);
            
            query = query.Where(
                interview => interview.Interviewers != null 
                && interview.Interviewers.Contains(interviewer!));
        }
        
        if (request.InterviewStatus != null)
        {
            query = query.Where(interview => interview.Status == request.InterviewStatus);
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query.Skip(request.PageSize * (request.PageNumber - 1))
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .Include(interview => interview.Job)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<InterviewViewModel>>(items);

        return new PaginatedResult<InterviewViewModel>(request.PageNumber, request.PageSize, total, result);
    }
}
