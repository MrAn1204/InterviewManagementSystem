using System.Linq.Expressions;

namespace IMS.Business.Services;

public interface IHangfireBackgroundService
{
    string ScheduleBackgroundJob(Expression<Func<Task>> job, DateTime scheduleAt);

    bool DeleteBackgroundJob(string backgroundJobId);
}
