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

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class InterviewsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new InterviewGetAllQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new InterviewGetByIdQuery { Id = id });
        return Ok(result);
    }

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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, InterviewCreateUpdateCommand request)
    {
        request.Id = id;

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        InterviewDeleteByIdCommand request = new()
        {
            Id = id
        };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("send-reminder")]
    public async Task<IActionResult> SendReminder([FromBody] InterviewRemindCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
