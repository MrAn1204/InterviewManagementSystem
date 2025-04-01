using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewCreateUpdateCommandHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewCreateUpdateCommand, InterviewViewModel>
{
    public Task<InterviewViewModel> Handle(InterviewCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            return Update(request, cancellationToken);
        }
        else
        {
            return Create(request, cancellationToken);
        }
    }

    private async Task<InterviewViewModel> Create(InterviewCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        var newInterview = _mapper.Map<Interview>(request);

        _unitOfWork.InterviewRepository.Add(newInterview);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new DatabaseBadRequestException("Create interview failed");
        }

        var createdInterview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .FirstOrDefaultAsync(interview => interview.Id == newInterview.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        return _mapper.Map<InterviewViewModel>(createdInterview);
    }

    private async Task<InterviewViewModel> Update(InterviewCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        var existedInterview = await _unitOfWork.InterviewRepository.GetByIdAsync(request.Id!.Value);

        if (existedInterview == null)
        {
            throw new ResourceNotFoundException("Interview not found");
        }

        _mapper.Map(request, existedInterview);

        _unitOfWork.InterviewRepository.Update(existedInterview);
        await _unitOfWork.SaveChangesAsync();

        var updatedInterview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Recruiter)
            .Include(interview => interview.Interviewers)
            .FirstOrDefaultAsync(interview => interview.Id == existedInterview.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        return _mapper.Map<InterviewViewModel>(updatedInterview);
    }
}
