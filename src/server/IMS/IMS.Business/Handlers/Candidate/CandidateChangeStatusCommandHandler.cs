using IMS.Business.Services;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateChangeStatusCommandHandler : IRequestHandler<CandidateChangeStatusCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWork;

    public CandidateChangeStatusCommandHandler(IUnitOfWorks unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(CandidateChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _unitOfWork.CandidateRepository.GetByIdAsync(request.Id) ?? throw new ResourceNotFoundException($"Candidate is not found");
        candidate.Status = request.Status;
        candidate.UpdatedDate = DateTime.Now;
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}
