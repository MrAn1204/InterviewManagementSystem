using System.Linq.Expressions;
using Hangfire;

namespace IMS.Business.Services;

public class HangfireBackgroundService : IHangfireBackgroundService
{
    public string ScheduleBackgroundJob(Expression<Func<Task>> job, DateTime scheduleAt)
    {
        return BackgroundJob.Schedule(job, scheduleAt);
    }

    public bool DeleteBackgroundJob(string backgroundJobId)
    {
        return BackgroundJob.Delete(backgroundJobId);
    }
}