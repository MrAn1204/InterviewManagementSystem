using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ViVuStore.Business.Handlers;

namespace IMS.API.Controllers;

[Route("api/jobs")]
[ApiController]
public class JobController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(JobCreateUpdateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new JobGetAllQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new JobGetByIdQuery { Id = id });

        if (result == null)
        {
            return NotFound($"Job with ID {id} not found.");
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(JobViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, JobCreateUpdateCommand command)
    {
        command.Id = id;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] JobSearchQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new JobDeleteByIdCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportJobs([FromForm] JobsImportFromExcelCommand command)
    {
        if (command.File == null || command.File.Length <= 0)
            return BadRequest("File is empty");

        if (!Path.GetExtension(command.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File is not an Excel file");

        var result = await _mediator.Send(command);
        
        if (result == null)
        {
            return BadRequest("Failed to import jobs.");
        }

        return Ok(result);
    }

}
