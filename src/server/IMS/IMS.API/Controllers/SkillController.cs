using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Controller for managing job skills.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SkillController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Retrieves all job skills.
    /// </summary>
    /// <returns>A list of job skills in JSON format.</returns>
    /// <response code="200">Returns the list of job skills.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(){
        var result=await _mediator.Send(new SkillGetAllQuery());
        return Ok(result);
    }
}
