using IMS.Business.Handlers;
using IMS.Business.Handlers.Level;
using IMS.Business.Handlers.Skill;
using IMS.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataForInputController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("Levels")]
    public async Task<IActionResult> GetAllLevels()
    {
        var result = await _mediator.Send(new LevelGetAllQuery());
        return Ok(result);
    }

    [HttpGet("Skills")]
    public async Task<IActionResult> GetAllSkills()
    {
        var result = await _mediator.Send(new SkillGetAllQuery());
        return Ok(result);
    }

    [HttpPost("UsersByRole")]
    public async Task<IActionResult> GetUserByRole([FromBody] GetUserByRoleQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

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
