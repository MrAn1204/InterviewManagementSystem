using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(request);

        return Ok(result);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _mediator.Send(new LogoutQuery());

        if (result)
        {
            return Ok(new { message = "Logout successful" });
        }

        return BadRequest(new { message = "User is not logged in" });
    }
}
