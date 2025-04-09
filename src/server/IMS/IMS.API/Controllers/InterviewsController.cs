using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using IMS.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Handles API requests for interviews.
/// </summary>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class InterviewsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Creates a new interview.
    /// </summary>
    /// <param name="request">The interview creation command containing interview details.</param>
    /// <returns>The created interview details.</returns>
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] InterviewCreateUpdateCommand request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of all interviews.
    /// </summary>
    /// <returns>A list of all interviews.</returns>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new InterviewGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a job by its ID.
    /// </summary>
    /// <param name="id">The ID of the job to retrieve.</param>
    /// <returns>The job details if found, otherwise a 404 error.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new InterviewGetByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Searches for interviews based on the provided query.
    /// </summary>
    /// <param name="keyword">The keyword to search for in the interview title.</param>
    /// <param name="interviewerId">The ID of the interviewer to search for.</param>
    /// <param name="interviewStatus">The status of the interview to search for.</param>
    /// <param name="pageNumber">The page number of results to return.</param>
    /// <param name="pageSize">The number of results to return per page.</param>
    /// <returns>A paginated list of interviews matching the search criteria.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? keyword,
        [FromQuery] int? interviewerId,
        [FromQuery] string? interviewStatus,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var request = new InterviewSearchQuery
        {
            Keyword = keyword,
            PageNumber = pageNumber,
            PageSize = pageSize,
            InterviewerId = interviewerId,
            InterviewStatus = interviewStatus
        };

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing interview by its ID.
    /// </summary>
    /// <param name="id">The ID of the interview to update.</param>
    /// <param name="request">The interview update request containing updated details.</param>
    /// <returns>The updated interview details if successful, otherwise a 404 or 400 error.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, InterviewCreateUpdateCommand request)
    {
        request.Id = id;

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    /// <summary>
    /// Sends an interview reminder to the candidate and the interviewers if the interview is within 24 hours.
    /// </summary>
    /// <param name="request">The interview reminder command containing the email, interview ID, and interview link.</param>
    /// <returns>A boolean indicating whether the reminder email has been sent successfully.</returns>
    [HttpPost("send-reminder")]
    public async Task<IActionResult> SendReminder([FromBody] InterviewRemindCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
