using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferGetByIdQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<OfferGetByIdQuery, OfferViewModel>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;


    public async Task<OfferViewModel> Handle(OfferGetByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery();

        var result = await query.Include(o => o.Department)
            .Include(o => o.Candidate).ThenInclude(c => c.HighestLevel)
            .Include(o => o.Interview)
            .Include(o => o.UserApproved).FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new ResourceNotFoundException("Offer not found");

        return _mapper.Map<OfferViewModel>(result);
    }
}
