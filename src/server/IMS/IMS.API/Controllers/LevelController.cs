using IMS.Business.Handlers.Level;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LevelController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var result=await _mediator.Send(new LevelGetAllQuery());
        return Ok(result);
    }
}
