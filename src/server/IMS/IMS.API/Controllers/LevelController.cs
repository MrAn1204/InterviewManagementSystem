using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Controller for managing job levels.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LevelController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Retrieves all job levels.
    /// </summary>
    /// <returns>A list of job levels in JSON format.</returns>
    /// <response code="200">Returns the list of job levels.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new LevelGetAllQuery());
        return Ok(result);
    }
}
