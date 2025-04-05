using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllDepartmentsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                Error = "Internal server error", 
                Message = ex.Message 
            });
        }
    }
}