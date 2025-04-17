using AutoMapper;
using Hangfire;
using Humanizer;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewRemindCommandHandler(
    IUnitOfWorks unitOfWork, IMapper mapper, 
    IEmailService emailService, IHangfireBackgroundService backgroundService) 
    : BaseHandler(unitOfWork, mapper), IRequestHandler<InterviewRemindCommand, bool>
{
    private readonly IEmailService _emailService = emailService;

    private readonly IHangfireBackgroundService _backgroundService = backgroundService;

    public int UserId { get; set; }

    public async Task<bool> Handle(InterviewRemindCommand request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Job)
            .Include(interview => interview.Recruiter)
            .FirstOrDefaultAsync(interview => interview.Id == request.InterviewId, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        if (interview.Status == InterviewStatus.Invited || interview.Status == InterviewStatus.Interviewed)
        {
            return false;
        }

        var scheduleAt = interview.InterviewDate.ToDateTime(interview.StartTime).AddDays(-1).At(8);

        bool reminderExisted = await _unitOfWork.ReminderRepository.GetQuery()
            .AnyAsync(reminder => reminder.InterviewId == request.InterviewId && reminder.Email == request.Email
                && reminder.ScheduleAt == scheduleAt && !reminder.IsDelete, cancellationToken);

        if (reminderExisted)
        {
            return false;
        }

        string subject = $"[no-reply-email-IMS-system] {interview.Title}";
        string message = $@"
            <html>
            <body>
                <p>This email is from <b>IMS system</b>.</p>
                <p>You have an interview schedule on {interview.InterviewDate} 
                    at {interview.StartTime} to {interview.EndTime}</p>
                <p>With Candidate {interview.Candidate!.FullName} position {interview.Job!.Title}.</p> 
                {interview.Candidate.CV ?? $@"<p>The CV of this candidate is attached 
                    <a href='{interview.Candidate!.CV}' style='color: blue; text-decoration: underline;'>here</a>.</p>"}
                {interview.MeetingID ?? $@"<p>Please join interview room ID: 
                    <a href='{interview.MeetingID} style='color: blue; text-decoration: underline;''></a>.</p>"}
                <p>If anything wrong, please refer recruiter {interview.Recruiter?.Email ?? interview.Recruiter?.FullName} 
                    or visit <a href='{request.InterviewLink}' style='color: blue; text-decoration: underline;'>our website</a>.</p>
                <br>
                <p>Thanks & Regards!<br>
                <b>IMS Team</b></p>
            </body>
            </html>";

        string backgroundJobId = _backgroundService.ScheduleBackgroundJob(
            () => _emailService.SendEmailAsync(request.Email, subject, message),
            scheduleAt);

        var reminder = new Reminder
        {
            Email = request.Email,
            Title = interview.Title,
            ScheduleAt = scheduleAt,
            BackgroundJobId = backgroundJobId,
            InterviewId = interview.Id
        };

        _unitOfWork.ReminderRepository.Add(reminder);
        var result = await _unitOfWork.SaveChangesAsync();

        return result > 0;
    }
}