using IMS.Business.Handlers;
using MediatR;
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
    public async Task<IActionResult> Logout([FromBody] LogoutCommand request)
    {
        var result = await _mediator.Send(request);

        if (result)
        {
            return Ok(new { message = "Logout successful" });
        }

        return BadRequest(new { message = "User is not logged in" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(request);

        return Ok(new { message = "Password reset email has been sent" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(request);

        if (result.Length > 0)
        {
            return BadRequest(new { message = $"[FAILED] {result}" });
        }

        return Ok(new { message = "[SUCCESS] Your password has been reset." });
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand request)
    {
        await _mediator.Send(request);

        return Ok(new { message = "Refresh token has been revoked." });
    }

    [HttpGet("validate-reset-password")]
    public async Task<IActionResult> ValidateResetPassword([FromBody] ValidateResetPasswordCommand request)
    {
        var result = await _mediator.Send(request);

        if (!result)
        {
            return BadRequest(new { message = "Reset password token is not valid." });
        }

        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }
}
