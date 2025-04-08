
using IMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IMS.Business.Services;

public class JobStatusUpdateBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<JobStatusUpdateBackgroundService> _logger;

    private readonly IJobStatusUpdateService updateService;

    public JobStatusUpdateBackgroundService(
        IServiceProvider services,
        ILogger<JobStatusUpdateBackgroundService> logger,
        IJobStatusUpdateService updateService)
    {
        _services = services;
        _logger = logger;
        this.updateService = updateService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Job Status Update Background Service is starting.");

        // Lên lịch chạy hàng ngày vào 00:01
        while (!stoppingToken.IsCancellationRequested)
        {
            // Tính toán thời gian còn lại đến 00:01 ngày hôm sau
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 0, 1, 0, DateTimeKind.Local).AddDays(1);
            var delay = nextRun - now;

            _logger.LogInformation("Next job status update scheduled at: {NextRun}", nextRun);

            await Task.Delay(delay, stoppingToken);

            try
            {
                using (var scope = _services.CreateScope())
                {
                    await updateService.UpdateJobStatusesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred updating job statuses.");
            }
        }
    }
}