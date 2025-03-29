using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // TODO: Removed this once this controller is fully implemented
    [HttpPost("create-user-mock")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateMock([FromBody] CreateMockUserCommand request)
    {
        var result = await _mediator.Send(request);

        if (result.Length > 0)
        {
            return BadRequest(new { message = $"[FAILED] {result}" });
        }

        return Ok(new { message = $"[SUCCESS] New mock user has been created." });
    }
}
