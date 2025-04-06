
using IMS.Data;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IMS.Business.Services;

public class JobStatusUpdateService : IJobStatusUpdateService
{
    private readonly IUnitOfWorks _unitOfWork;
    private readonly ILogger<JobStatusUpdateService> _logger;

    public JobStatusUpdateService(
        IUnitOfWorks unitOfWork,
        ILogger<JobStatusUpdateService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task UpdateJobStatusesAsync()
    {
        _logger.LogInformation("Starting job status update process...");

        var today = DateTime.Today;

        // Cập nhật từ Draft sang Open cho các công việc có startDate = today
        var draftsToOpen = await _unitOfWork.JobRepository.GetQuery()
            .Where(j => j.Status == "Draft" && j.StartDate == today)
            .ToListAsync();

        foreach (var job in draftsToOpen)
        {
            job.Status = "Open";
            _logger.LogInformation($"Updating job {job.Id} from Draft to Open");
        }

        // Cập nhật từ Open sang Closed cho các công việc có endDate = today
        var opensToClosed = await _unitOfWork.JobRepository.GetQuery()
            .Where(j => j.Status == "Open" && j.EndDate < today)
            .ToListAsync();

        foreach (var job in opensToClosed)
        {
            job.Status = "Closed";
            _logger.LogInformation($"Updating job {job.Id} from Open to Closed");
        }

        // Lưu các thay đổi vào database
        var updatedCount = await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation($"Updated {updatedCount} jobs in total");
    }
}