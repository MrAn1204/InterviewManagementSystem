using System;
using IMS.Business.ViewModels.Offer;
using MediatR;

namespace IMS.Business.Handlers.Offer;

public class OfferGetByIdQuery : IRequest<OfferViewModel>
{
    public int Id { get; set; }
}
