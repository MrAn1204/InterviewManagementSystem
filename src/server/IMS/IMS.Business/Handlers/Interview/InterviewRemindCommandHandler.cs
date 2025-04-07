using Hangfire;
using Humanizer;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewRemindCommandHandler(
    IUnitOfWorks unitOfWorks, IEmailService emailService) : IRequestHandler<InterviewRemindCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

    private readonly IEmailService _emailService = emailService;

    public int UserId { get; set; }

    public async Task<bool> Handle(InterviewRemindCommand request, CancellationToken cancellationToken)
    {
        var interview = await _unitOfWorks.InterviewRepository.GetQuery()
            .Include(interview => interview.Candidate)
            .Include(interview => interview.Job)
            .Include(interview => interview.Recruiter)
            .FirstOrDefaultAsync(interview => interview.Id == request.InterviewId, cancellationToken)
            ?? throw new ResourceNotFoundException("Interview not found");

        var scheduleAt = interview.InterviewDate.ToDateTime(interview.StartTime).AddDays(-1).At(22, 56);

        bool reminderExisted = await _unitOfWorks.ReminderRepository.GetQuery()
            .AnyAsync(reminder => reminder.Email == request.Email
                && reminder.ScheduleAt == scheduleAt, cancellationToken);

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
                <p>With Candidate {interview.Candidate!.FullName} position {interview.Job!.Title}, 
                    the CV is attached with this no-reply-email.</p>
                <p>If anything wrong, please refer recruiter {interview.Recruiter?.Email ?? interview.Recruiter?.FullName} 
                    or visit <a href='{request.InterviewLink}' style='color: blue; text-decoration: underline;'>our website</a>.</p>
                {interview.MeetingID ?? $@"<p>Please join interview room ID: 
                    <a href='{interview.MeetingID} style='color: blue; text-decoration: underline;''></a>.</p>"}
                <br>
                <p>Thanks & Regards!<br>
                <b>IMS Team</b></p>
            </body>
            </html>";

        string backgroundJobId = BackgroundJob.Schedule(
            () => _emailService.SendEmailAsync(request.Email, subject, message),
            scheduleAt);

        var reminder = new Reminder
        {
            Email = request.Email,
            Title = interview.Title,
            ScheduleAt = scheduleAt,
            BackgroundJobId = backgroundJobId
        };

        _unitOfWorks.ReminderRepository.Add(reminder);
        var result = await _unitOfWorks.SaveChangesAsync();

        return result > 0;
    }
}