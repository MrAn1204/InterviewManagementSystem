using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ViVuStore.Business.Handlers;

public class JobsImportFromExcelCommand : IRequest<JobImportResult>
{
    public required IFormFile File { get; set; }
    public int CreatedBy { get; set; }
}
