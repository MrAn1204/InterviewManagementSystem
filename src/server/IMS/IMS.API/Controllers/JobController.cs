using IMS.Business.Handlers;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ViVuStore.Business.Handlers;

namespace IMS.API.Controllers;

/// <summary>
/// API controller for managing job resources.
/// Provides endpoints for creating, retrieving, updating, searching, deleting, and importing jobs.
/// Also includes an endpoint for manually triggering job status updates.
/// </summary>
[Route("api/jobs")]
[ApiController]
public class JobController(IMediator mediator, IJobStatusUpdateService jobStatusService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IJobStatusUpdateService _jobStatusService = jobStatusService;

    /// <summary>
    /// Creates a new job.
    /// </summary>
    /// <param name="command">The job creation command containing job details.</param>
    /// <returns>The created job details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(JobCreateUpdateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all jobs.
    /// </summary>
    /// <returns>A list of all jobs.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new JobGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a job by its ID.
    /// </summary>
    /// <param name="id">The ID of the job to retrieve.</param>
    /// <returns>The job details if found, otherwise a 404 error.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new JobGetByIdQuery { Id = id });

        if (result == null)
        {
            return NotFound($"Job with ID {id} not found.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing job by its ID.
    /// </summary>
    /// <param name="id">The ID of the job to update.</param>
    /// <param name="command">The job update command containing updated details.</param>
    /// <returns>The updated job details if successful, otherwise a 404 or 400 error.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(JobViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, JobCreateUpdateCommand command)
    {
        command.Id = id;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Searches for jobs based on the provided query.
    /// </summary>
    /// <param name="query">The search query containing filters and criteria.</param>
    /// <returns>A list of jobs matching the search criteria.</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromBody] JobSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a job by its ID.
    /// </summary>
    /// <param name="id">The ID of the job to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new JobDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Imports jobs from an Excel file.
    /// </summary>
    /// <param name="command">The command containing the Excel file to import.</param>
    /// <returns>The result of the import operation.</returns>
    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportJobs([FromForm] JobsImportFromExcelCommand command)
    {
        if (command.File == null || command.File.Length <= 0)
            return BadRequest("File is empty");

        if (!Path.GetExtension(command.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File is not an Excel file");

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return BadRequest("Failed to import jobs.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Manually updates job statuses.
    /// </summary>
    /// <returns>A success message indicating the statuses were updated.</returns>
    [HttpPost("updateStatus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ManualUpdate()
    {
        await _jobStatusService.UpdateJobStatusesAsync();
        return Ok(new { message = "Job statuses updated successfully" });
    }

}
