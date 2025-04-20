using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ViVuStore.Business.Handlers;

public class JobsImportFromExcelCommandHandler(IUnitOfWorks unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<JobsImportFromExcelCommand, JobImportResult>
{
    public async Task<JobImportResult> Handle(
        JobsImportFromExcelCommand request, CancellationToken cancellationToken)
    {
        var result = new JobImportResult
        {
            TotalRows = 0,
            ImportedRows = 0,
            SkippedRows = 0,
            Errors = []
        };
        ExcelPackage.License.SetNonCommercialOrganization("IMS");

        using (var stream = new MemoryStream())
        {
            await request.File.CopyToAsync(stream, cancellationToken);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            result.TotalRows = rowCount - 1;

            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var title = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
                    var workingAddress = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
                    var skillsString = worksheet.Cells[row, 8].Value?.ToString()?.Trim() ?? string.Empty;
                    var benefitsString = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? string.Empty;
                    var levelsString = worksheet.Cells[row, 10].Value?.ToString()?.Trim() ?? string.Empty;

                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(workingAddress))
                    {
                        result.Errors.Add($"Row {row}: Title or Working Address is empty");
                        result.SkippedRows++;
                        continue;
                    }

                    (bool flowControlSalary, (decimal salaryMin, decimal salaryMax)) = ParseSalary(result, worksheet, row);
                    if (!flowControlSalary)
                    {
                        continue;
                    }

                    (bool flowControlDate, (DateTime? startDate, DateTime? endDate)) = ParseDate(result, worksheet, row);
                    if (!flowControlDate)
                    {
                        continue;
                    }

                    var existingJob = await _unitOfWork.JobRepository.GetQuery()
                                .FirstOrDefaultAsync(j => j.Title == title && j.WorkingAddress == workingAddress, cancellationToken);

                    if (existingJob != null)
                    {
                        result.SkippedRows++;
                        continue;
                    }

                    var newJob = new Job
                    {
                        Title = title,
                        WorkingAddress = workingAddress,
                        SalaryMin = salaryMin,
                        SalaryMax = salaryMax,
                        Description = worksheet.Cells[row, 5].Value?.ToString(),
                        StartDate = startDate,
                        EndDate = endDate,
                        Status = "Open",
                        CreatedBy = request.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    await AddEntitiesToJobAsync<Skill>(
                        skillsString,
                        async (name, ct) => await _unitOfWork.GenericRepository<Skill>().GetQuery()
                            .FirstOrDefaultAsync(s => s.SkillName == name, ct),
                        name => new Skill { SkillName = name },
                        skill => _unitOfWork.GenericRepository<Skill>().Add(skill),
                        skill => newJob.JobSkills.Add(new JobSkill { SkillId = skill.Id, Skill = skill }),
                        cancellationToken
                    );

                    await AddEntitiesToJobAsync<Benefit>(
                        benefitsString,
                        async (name, ct) => await _unitOfWork.GenericRepository<Benefit>().GetQuery()
                            .FirstOrDefaultAsync(b => b.BenefitName == name, ct),
                        name => new Benefit { BenefitName = name },
                        benefit => _unitOfWork.GenericRepository<Benefit>().Add(benefit),
                        benefit => newJob.JobBenefits.Add(new JobBenefit { BenefitId = benefit.Id, Benefit = benefit }),
                        cancellationToken
                    );

                    await AddEntitiesToJobAsync<Level>(
                        levelsString,
                        async (name, ct) => await _unitOfWork.GenericRepository<Level>().GetQuery()
                            .FirstOrDefaultAsync(l => l.LevelName == name, ct),
                        name => new Level { LevelName = name },
                        level => _unitOfWork.GenericRepository<Level>().Add(level),
                        level => newJob.JobLevels.Add(new JobLevel { LevelId = level.Id, Level = level }),
                        cancellationToken
                    );

                    _unitOfWork.JobRepository.Add(newJob);
                    result.ImportedRows++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Row {row}: {ex.Message}");
                    result.SkippedRows++;
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }

    private static async Task AddEntitiesToJobAsync<TEntity>(
        string inputString,
        Func<string, CancellationToken, Task<TEntity?>> findEntityAsync,
        Func<string, TEntity> createEntity,
        Action<TEntity> addEntityToRepository,
        Action<TEntity> addRelationToJob,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        if (string.IsNullOrEmpty(inputString)) return;

        var items = inputString.Split(',').Select(s => s.Trim()).ToList();

        foreach (var itemName in items)
        {
            var entity = await findEntityAsync(itemName, cancellationToken);
            if (entity == null)
            {
                entity = createEntity(itemName);
                addEntityToRepository(entity);
            }
            addRelationToJob(entity);
        }
    }

    private static (bool flowControl, (decimal salaryMin, decimal salaryMax) value) ParseSalary(JobImportResult result, ExcelWorksheet worksheet, int row)
    {
        if (!decimal.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out decimal salaryMin))
        {
            result.Errors.Add($"Row {row}: Invalid Salary Min format");
            result.SkippedRows++;
            return (flowControl: false, value: default);
        }

        if (!decimal.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out decimal salaryMax))
        {
            result.Errors.Add($"Row {row}: Invalid Salary Max format");
            result.SkippedRows++;
            return (flowControl: false, value: default);
        }

        if (salaryMin > salaryMax)
        {
            result.Errors.Add($"Row {row}: Salary Min cannot be greater than Salary Max");
            result.SkippedRows++;
            return (flowControl: false, value: default);
        }

        return (flowControl: true, value: (salaryMin, salaryMax));
    }

    private static (bool flowControl, (DateTime? startDate, DateTime? endDate) value)
        ParseDate(JobImportResult result, ExcelWorksheet worksheet, int row)
    {
        DateTime? startDate = null;
        if (worksheet.Cells[row, 6].Value != null)
        {
            try
            {
                if (double.TryParse(worksheet.Cells[row, 6].Value.ToString(), out double excelDateValue))
                {
                    startDate = DateTime.FromOADate(excelDateValue);
                }
                else
                {
                    startDate = DateTime.Parse(worksheet.Cells[row, 6].Value?.ToString() ?? string.Empty,
                                              System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                result.Errors.Add($"Row {row}: Invalid Start Date format");
                result.SkippedRows++;
                return (flowControl: false, value: default);
            }
        }

        DateTime? endDate = null;
        if (worksheet.Cells[row, 7].Value != null)
        {
            try
            {
                if (double.TryParse(worksheet.Cells[row, 7].Value.ToString(), out double excelDateValue))
                {
                    endDate = DateTime.FromOADate(excelDateValue);
                }
                else
                {
                    endDate = DateTime.Parse(worksheet.Cells[row, 7].Value?.ToString() ?? string.Empty,
                                              System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                result.Errors.Add($"Row {row}: Invalid End Date format");
                result.SkippedRows++;
                return (flowControl: false, value: default);
            }
        }

        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            result.Errors.Add($"Row {row}: Start Date cannot be after End Date");
            result.SkippedRows++;
            return (flowControl: false, value: default);
        }

        return (flowControl: true, value: (startDate, endDate));
    }
}