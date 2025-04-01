using IMS.Business.Handlers.Skill;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var result=await _mediator.Send(new SkillGetAllQuery());
        return Ok(result);
    }
}
