using System;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers.Offer;

public class OfferCreateCommandHandler : IRequestHandler<OfferCreateCommand, int>
{
    private readonly IUnitOfWorks _unitOfWork;
    public OfferCreateCommandHandler(IUnitOfWorks unitOfWorks)
    {
        _unitOfWork = unitOfWorks;
    }

    public Task<int> Handle(OfferCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
