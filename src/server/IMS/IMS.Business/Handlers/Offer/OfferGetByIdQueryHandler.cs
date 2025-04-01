using System;
using AutoMapper;
using IMS.Business.ViewModels.Offer;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Offer;

public class OfferGetByIdQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<OfferGetByIdQuery, OfferViewModel>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;


    public async Task<OfferViewModel> Handle(OfferGetByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery();

        var result = await query.Include(o => o.OfferDepartments)
            .ThenInclude(od => od.Department).FirstOrDefaultAsync(of => of.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Candidate not found");

        return _mapper.Map<OfferViewModel>(result);
    }
}
