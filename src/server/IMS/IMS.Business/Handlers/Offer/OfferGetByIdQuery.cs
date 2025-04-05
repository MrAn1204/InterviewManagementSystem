using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferGetByIdQuery : IRequest<OfferViewModel>
{
    public int Id { get; set; }
}
