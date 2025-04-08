using System;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferChangeStatusCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Status { get; set; } = "WaitingForApproval";
}
