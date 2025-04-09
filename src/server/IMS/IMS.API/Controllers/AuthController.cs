using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// Handles authentication-related API requests.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Logs a user into the system. The login request is represented by the
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>A response containing the access token and refresh token
    /// if the login is successful, otherwise returns a response indicating the
    /// reason for the failure.</returns>
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

    /// <summary>
    /// Logs a user out of the system. The logout request is represented by the
    /// </summary>
    /// <param name="request">The logout request.</param>
    /// <returns>A response indicating success or failure of the logout operation.
    /// On success, returns a message indicating successful logout; on failure,
    /// returns a message indicating the user is not logged in.</returns>
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

    /// <summary>
    /// Sends a password reset email to the email address specified in the
    /// </summary>
    /// <param name="request">The request containing the email address.</param>
    /// <returns>A response indicating success or failure of the password reset request.
    /// On success, returns a message indicating successful password reset email has been sent;
    /// on failure, returns a message indicating the request was invalid or the user was not found.</returns>
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

    /// <summary>
    /// Resets a user's password using the token and new password provided in the
    /// </summary>
    /// <param name="request">The request containing the reset password token and new password details.</param>
    /// <returns>A response indicating success or failure of the password reset operation.
    /// On success, returns a message indicating the password has been reset; on failure,
    /// returns a message explaining the reason for failure.</returns>
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

    /// <summary>
    /// Revokes a user's refresh token, preventing them from generating a new
    /// access token from the revoked refresh token.
    /// </summary>
    /// <param name="request">The request containing the refresh token to be revoked.</param>
    /// <returns>A response indicating success of the revocation operation.</returns>
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand request)
    {
        await _mediator.Send(request);

        return Ok(new { message = "Refresh token has been revoked." });
    }

    /// <summary>
    /// Validates a reset password token to ensure it is valid and can be used to reset a user's password.
    /// </summary>
    /// <param name="request">The request containing the reset password token to be validated.</param>
    /// <returns>A status code indicating success or failure of the validation operation.
    /// On success, returns a 200 (OK) status code; on failure, returns a 400 (BAD REQUEST) status code.</returns>
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

    /// <summary>
    /// Generates a new access token from a refresh token.
    /// </summary>
    /// <param name="request">The request containing the refresh token to be used.</param>
    /// <returns>A response containing the new access token.</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }
}
