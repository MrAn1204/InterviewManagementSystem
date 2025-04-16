using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Handles all candidate-related operations including create, update, delete, and retrieval.
/// </summary>
/// <param name="mediator">The mediator instance for sending commands and queries.</param>
[Route("api/[controller]")]
[ApiController]
public class CandidateController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Creates a new candidate.
    /// </summary>
    /// <param name="command">The command containing candidate creation data.</param>
    /// <returns>Returns the result of the candidate creation operation.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> Create([FromForm] CandidateCreateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all candidates.
    /// </summary>
    /// <returns>Returns a list of all candidates.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCandidateQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific candidate by ID.
    /// </summary>
    /// <param name="id">The ID of the candidate to retrieve.</param>
    /// <returns>Returns the candidate details.</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCandidateByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Searches for candidates based on filter criteria.
    /// </summary>
    /// <param name="query">The search query parameters.</param>
    /// <returns>Returns a list of candidates that match the search criteria.</returns>
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchCandidateQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing candidate's information.
    /// </summary>
    /// <param name="command">The command containing updated candidate data.</param>
    /// <returns>Returns the result of the update operation.</returns>
    [HttpPost("update")]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> Update([FromForm] CandidateUpdateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Changes the status of a candidate (e.g., Active, Inactive).
    /// </summary>
    /// <param name="command">The command containing status change data.</param>
    /// <returns>Returns the result of the status update operation.</returns>
    [HttpPost("status")]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> ChangeStastus([FromBody] CandidateChangeStatusCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a candidate.
    /// </summary>
    /// <param name="command">The command containing the ID of the candidate to delete.</param>
    /// <returns>Returns the result of the delete operation.</returns>
    [HttpPost("delete")]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> Delete([FromBody] CandidateDeleteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Gets the number of candidates grouped by week for a specific month and year.
    /// </summary>
    /// <param name="year">The year to filter by.</param>
    /// <param name="month">The month to filter by.</param>
    /// <returns>Returns a weekly count of candidates.</returns>
    [HttpGet("count-by-week")]
    public async Task<IActionResult> GetCandidateCountByWeek(int year, int month)
    {
        var result = await _mediator.Send(new CandidateCountByWeekQuery { Year = year, Month = month });
        return Ok(result);
    }

}
