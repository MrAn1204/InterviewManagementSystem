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

                    decimal salaryMin = 0;
                    if (!decimal.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out salaryMin))
                    {
                        result.Errors.Add($"Row {row}: Invalid Salary Min format");
                        result.SkippedRows++;
                        continue;
                    }

                    decimal salaryMax = 0;
                    if (!decimal.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out salaryMax))
                    {
                        result.Errors.Add($"Row {row}: Invalid Salary Max format");
                        result.SkippedRows++;
                        continue;
                    }

                    if (salaryMin > salaryMax)
                    {
                        result.Errors.Add($"Row {row}: Salary Min cannot be greater than Salary Max");
                        result.SkippedRows++;
                        continue;
                    }

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
                            continue;
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
                            continue;
                        }
                    }

                    if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                    {
                        result.Errors.Add($"Row {row}: Start Date cannot be after End Date");
                        result.SkippedRows++;
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

                    if (!string.IsNullOrEmpty(skillsString))
                    {
                        var skills = skillsString.Split(',').Select(s => s.Trim()).ToList();
                        foreach (var skillName in skills)
                        {
                            var skill = await _unitOfWork.GenericRepository<Skill>().GetQuery().FirstOrDefaultAsync(s => s.SkillName == skillName, cancellationToken);
                            if (skill == null)
                            {
                                skill = new Skill { SkillName = skillName };
                                _unitOfWork.GenericRepository<Skill>().Add(skill);
                            }
                            newJob.JobSkills.Add(new JobSkill { SkillId = skill.Id, Skill = skill });
                        }
                    }

                    if (!string.IsNullOrEmpty(benefitsString))
                    {
                        var benefits = benefitsString.Split(',').Select(b => b.Trim()).ToList();
                        foreach (var benefitName in benefits)
                        {
                            var benefit = await _unitOfWork.GenericRepository<Benefit>().GetQuery().FirstOrDefaultAsync(b => b.BenefitName == benefitName, cancellationToken);
                            if (benefit == null)
                            {
                                benefit = new Benefit { BenefitName = benefitName };
                                _unitOfWork.GenericRepository<Benefit>().Add(benefit);
                            }
                            newJob.JobBenefits.Add(new JobBenefit { BenefitId = benefit.Id, Benefit = benefit });
                        }
                    }

                    if (!string.IsNullOrEmpty(levelsString))
                    {
                        var levels = levelsString.Split(',').Select(l => l.Trim()).ToList();
                        foreach (var levelName in levels)
                        {
                            var level = await _unitOfWork.GenericRepository<Level>().GetQuery()
                                .FirstOrDefaultAsync(l => l.LevelName == levelName, cancellationToken);
                            if (level == null)
                            {
                                level = new Level { LevelName = levelName };
                                _unitOfWork.GenericRepository<Level>().Add(level);
                            }
                            newJob.JobLevels.Add(new JobLevel { LevelId = level.Id, Level = level });
                        }
                    }

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
}