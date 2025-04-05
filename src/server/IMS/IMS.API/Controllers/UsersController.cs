using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser([FromBody] CreateMockUserCommand request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid input data" });

        var result = await _mediator.Send(request);

        return result switch
        {
            "" => Ok(new { message = "Successfully created user" }),
            "Invalid department" => BadRequest(new { message = "ME028: Invalid department" }),
            "One or more roles are invalid" => BadRequest(new { message = "ME029: Invalid role(s)" }),
            _ => BadRequest(new { message = $"ME026: Failed to create user - {result}" })
        };
    }

    [HttpGet("list")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUserList([FromQuery] GetUserListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // UsersController.cs
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
    {
        try
        {
            command.UserId = id;
            await _mediator.Send(command);
            return Ok(new { Message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = "Failed to update user",
                ex.Message
            });
        }
    }
    // UsersController.cs
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = id });
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(new { 
                Error = "User not found",
                ex.Message 
            });
        }
    }

    [HttpPut("{id}/inactive")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> InactiveUser(int id)
    {
        try
        {
            await _mediator.Send(new InactiveUserCommand { UserId = id });
            return Ok(new { Message = "User inactivated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                Error = "Failed to inactivate user", 
                Message = ex.Message 
            });
        }
    }
}
