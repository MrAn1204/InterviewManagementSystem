using IMS.Business.Handlers;
using IMS.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Provides common lookup endpoints such as levels, skills, user roles, and candidate statuses.
/// </summary>
/// <param name="mediator">The mediator used to dispatch queries.</param>
[Route("api/[controller]")]
[ApiController]
public class CommonController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Retrieves all available levels
    /// </summary>
    /// <returns>List of levels.</returns>
    [HttpGet("Levels")]
    public async Task<IActionResult> GetAllLevels()
    {
        var result = await _mediator.Send(new LevelGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all available skills
    /// </summary>
    /// <returns>List of skills.</returns>
    [HttpGet("Skills")]
    public async Task<IActionResult> GetAllSkills()
    {
        var result = await _mediator.Send(new SkillGetAllQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves users based on their assigned roles.
    /// </summary>
    /// <param name="query">The role-based query containing filtering parameters.</param>
    /// <returns>List of users with the specified role.</returns>
    [HttpPost("UsersByRole")]
    public async Task<IActionResult> GetUserByRole([FromBody] GetUserByRoleQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a predefined list of selectable candidate statuses.
    /// </summary>
    /// <returns>List of candidate statuses allowed to be selected in forms or filters.</returns>
    [HttpGet("getSelectableCandidateStatuses")]
    public async Task<IActionResult> GetSelectableCandidateStatuses()
    {
        var selectableStatuses = new List<CandidateStatus>
        {
        CandidateStatus.Open,
        CandidateStatus.Banned,
        };

        var response = selectableStatuses
                       .Select(s => new { Id = (int)s, Name = s.ToString() })
                       .ToList();

        return Ok(response);
    }

    /// <summary>
    /// Retrieves all candidate statuses defined in the system.
    /// </summary>
    /// <returns>List of all candidate statuses.</returns>
    [HttpGet("getAllCandidateStatuses")]
    public async Task<IActionResult> GetAllCandidateStatuses()
    {
        var statuses = Enum.GetValues(typeof(CandidateStatus))
                           .Cast<CandidateStatus>()
                           .Select(s => new { Id = (int)s, Name = s.ToString() })
                           .ToList();

        return Ok(statuses);
    }
}
