using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Controller for managing job benefits.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BenefitController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Retrieves all job benefits.
    /// </summary>
    /// <returns>A list of job benefits in JSON format.</returns>
    /// <response code="200">Returns the list of job benefits.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new BenefitGetAllQuery());
        return Ok(result);
    }
}
