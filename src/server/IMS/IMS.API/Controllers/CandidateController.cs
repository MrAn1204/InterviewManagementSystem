using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidateController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCandidateQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCandidateByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchCandidateQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

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

    [HttpPost("status")]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> ChangeStastus([FromBody] CandidateChangeStatusCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("delete")]
    [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
    public async Task<IActionResult> Delete([FromBody] CandidateDeleteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("count-by-week")]
    public async Task<IActionResult> GetCandidateCountByWeek(int year, int month)
    {
        var result =await _mediator.Send(new CandidateCountByWeekQuery { Year = year, Month = month });
        return Ok(result);
    }

}
