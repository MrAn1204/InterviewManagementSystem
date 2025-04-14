using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/users")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("create")]
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("list")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUserList([FromQuery] GetUserListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
            return NotFound(new
            {
                Error = "User not found",
                ex.Message
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
            return StatusCode(500, new
            {
                Error = "Failed to inactivate user",
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/active")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ActiveUser(int id)
    {
        try
        {
            await _mediator.Send(new ActiveUserCommand { UserId = id });
            return Ok(new { Message = "User activated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = "Failed to activate user",
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    [HttpGet("by-roles")]
    public async Task<IActionResult> GetUsersByRoles([FromQuery] List<string> roles)
    {
        if (roles == null || roles.Count == 0)
        {
            return BadRequest("At least one role must be specified.");
        }

        var query = new GetUserByRoleQuery { Roles = roles };
        var result = await _mediator.Send(query);

        return Ok(result);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("check-unique")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckUnique([FromQuery] string? username, [FromQuery] string? email)
    {
        var result = await _mediator.Send(new CheckUniqueQuery 
        { 
            Username = username, 
            Email = email 
        });

        return Ok(result);
    }
    
    
}
