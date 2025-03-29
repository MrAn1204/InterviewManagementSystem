using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CandidateController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CandidateViewModel),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm]CandidateCreateCommand command){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        var result=_mediator.Send(command);
        return Ok(result);
    }
}

internal class CandidateViewModel
{
}
