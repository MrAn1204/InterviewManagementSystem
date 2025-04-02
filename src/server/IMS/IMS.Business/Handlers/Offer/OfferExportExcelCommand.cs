using System;
using IMS.Business.DTOs;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferExportExcelCommand : IRequest<byte[]>
{
    public DateTime? fromDate { get; set; }
    public DateTime? toDate { get; set; }
}
