using System.Security.Claims;
using AutoMapper;
using Hangfire;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewCreateUpdateCommandHandler(
    IUnitOfWorks unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewCreateUpdateCommand, InterviewViewModel>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("No active HttpContext. This operation requires an active HTTP request.");
        }

        var currentUser = _httpContextAccessor.HttpContext.User;

        var usersList = await _unitOfWork.Context.Users.ToListAsync(cancellationToken);

        var newInterview = _mapper.Map<Interview>(request, opts =>
        {
            opts.Items["Users"] = usersList;
        });

        newInterview.CreatedBy = Convert.ToInt32(currentUser.FindFirstValue(ClaimTypes.NameIdentifier));

        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
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
                .Include(interview => interview.Job)
                .FirstOrDefaultAsync(interview => interview.Id == newInterview.Id, cancellationToken)
                ?? throw new ResourceNotFoundException("Interview not found");

            createdInterview.Candidate!.Status = "Waiting for interview";
            await _unitOfWork.SaveChangesAsync();

            await transaction.CommitAsync(cancellationToken);

            return _mapper.Map<InterviewViewModel>(createdInterview);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<InterviewViewModel> Update(InterviewCreateUpdateCommand request, CancellationToken cancellationToken)
    {
        var existedInterview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.UserCreated)
            .Include(interview => interview.Interviewers)
            .FirstOrDefaultAsync(interview => interview.Id == request.Id, cancellationToken);


        if (existedInterview == null)
        {
            throw new ResourceNotFoundException("Interview not found");
        }

        var createdBy = existedInterview.CreatedBy;
        var usersList = await _unitOfWork.Context.Users.ToListAsync(cancellationToken);

        existedInterview.Interviewers!.Clear();
        _mapper.Map(request, existedInterview, opts =>
        {
            opts.Items["Users"] = usersList;
        });
        existedInterview.CreatedBy = createdBy;
        existedInterview.UpdatedDate = DateTime.UtcNow;

        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _unitOfWork.InterviewRepository.Update(existedInterview);
            await _unitOfWork.SaveChangesAsync();

            var updatedInterview = await _unitOfWork.InterviewRepository.GetQuery()
                .Include(interview => interview.Candidate)
                .Include(interview => interview.Recruiter)
                .Include(interview => interview.Interviewers)
                .Include(interview => interview.Job)
                .FirstOrDefaultAsync(interview => interview.Id == existedInterview.Id, cancellationToken)
                ?? throw new ResourceNotFoundException("Interview not found");

            if (updatedInterview.Result.HasValue)
            {
                var result = updatedInterview.Result == InterviewResult.Passed
                    ? "Passed Interview" : "Failed Interview";
                updatedInterview.Candidate!.Status = result;
                _unitOfWork.CandidateRepository.Update(updatedInterview.Candidate);
            }

            // Cancel sending reminders
            if (updatedInterview.Status == InterviewStatus.Cancelled)
            {
                var emails = updatedInterview.Interviewers!.Select(interviewer => interviewer.Email);
                var reminders = _unitOfWork.ReminderRepository.GetQuery().Where(reminder => emails.Contains(reminder.Email));

                foreach (var reminder in reminders)
                {
                    BackgroundJob.Delete(reminder.BackgroundJobId);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            await transaction.CommitAsync(cancellationToken);

            return _mapper.Map<InterviewViewModel>(updatedInterview);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
