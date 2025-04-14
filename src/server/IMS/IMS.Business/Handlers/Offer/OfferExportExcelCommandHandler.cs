using System;
using IMS.Business.DTOs;
using IMS.Business.Services;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferExportExcelCommandHandler(IUnitOfWorks unitOfWork, IFileService fileService) : IRequestHandler<OfferExportExcelCommand, byte[]>
{
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;
    private readonly IFileService _fileService = fileService;
    public Task<byte[]> Handle(OfferExportExcelCommand request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery(o => (!request.fromDate.HasValue || o.CreatedDate >= request.fromDate.Value) &&
            (!request.toDate.HasValue || o.CreatedDate <= request.toDate.Value));

        var result = query.Include(o => o.Candidate)
                  .Include(o => o.UserApproved)
                  .Include(o => o.Department)
                  .Select(o => new OfferExcelDto
                  {
                      candidateName = o.Candidate != null ? o.Candidate.FullName : "",
                      email = o.Candidate != null ? o.Candidate.Email : "",
                      approver = o.UserApproved != null ? o.UserApproved.FullName : "",
                      department = o.Department != null ? o.Department.DepartmentName : "",
                      notes = o.Note != null ? o.Note : "",
                      status = o.Status
                  })
                  .ToList();


        return _fileService.GenerateOfferExcelFile(result);
    }
}
