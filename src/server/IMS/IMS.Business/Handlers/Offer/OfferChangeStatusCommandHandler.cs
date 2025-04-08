using System;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferChangeStatusCommandHandler(IUnitOfWorks unitOfWork) : IRequestHandler<OfferChangeStatusCommand, bool>
{
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<bool> Handle(OfferChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.Id) ?? throw new ResourceNotFoundException($"Offer is not found");
        offer.Status = request.Status;
        offer.UpdatedDate = DateTime.Now;
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}
