using System;
using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.VisualBasic;

namespace IMS.Business.Handlers;

public class CandidateDeleteCommandHandler : IRequestHandler<CandidateDeleteCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWork;
    private readonly IFileService _fileService;

    public CandidateDeleteCommandHandler(IUnitOfWorks unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<bool> Handle(CandidateDeleteCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(request.Id) ?? throw new ResourceNotFoundException("Candidate not found");
        var isFileDeleted = false;
        if (candidate.CV != null)
        {
            isFileDeleted = await _fileService.DeleteFileAsync(candidate.CV);
        }
        if (isFileDeleted == true)
        {
            _unitOfWork.CandidateRepository.Delete(candidate);
        }
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}
