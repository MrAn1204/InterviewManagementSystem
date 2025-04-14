using IMS.Business.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new OfferGetAllQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new OfferGetByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
        public async Task<IActionResult> Create([FromBody] OfferCreateCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN, MANAGER, RECRUITER")]
        public async Task<IActionResult> EditOffer([FromBody] OfferUpdateCommand command)
        {
            var updatedOffer = await _mediator.Send(command);
            return Ok(updatedOffer);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchOffers([FromBody] OfferSearchQuery searchQuery)
        {
            var result = await _mediator.Send(searchQuery);
            return Ok(result);
        }

        [HttpPost("status")]
        [Authorize(Roles = "ADMIN, RECRUITER, MANAGER")]
        public async Task<IActionResult> ChangeStastus([FromBody] OfferChangeStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("export")]
        public async Task<IActionResult> ExportOfferExcel([FromBody] OfferExportExcelCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                // Gọi hàm xử lý từ handler để tạo tệp Excel
                byte[] excelFile = await _mediator.Send(command);

                // Trả về tệp Excel dưới dạng file
                return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "offers.xlsx");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
