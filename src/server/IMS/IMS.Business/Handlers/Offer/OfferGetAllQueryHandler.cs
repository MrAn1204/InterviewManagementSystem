using System;
using AutoMapper;
using IMS.Business.ViewModels.Offer;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Offer;

public class OfferGetAllQueryHandler : IRequestHandler<OfferGetAllQuery, IEnumerable<OfferViewModel>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorks _unitOfWork;

    public OfferGetAllQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OfferViewModel>> Handle(OfferGetAllQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.OfferRepository.GetQuery();

        var result = await query.Include(o => o.Department).ToListAsync();

        return _mapper.Map<IEnumerable<OfferViewModel>>(result);
    }
}
