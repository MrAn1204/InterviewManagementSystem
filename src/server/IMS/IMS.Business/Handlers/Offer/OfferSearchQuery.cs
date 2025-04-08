using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class OfferSearchQuery : BaseSearchQuery<OfferViewModel>
{
    // public string? Keyword { get; set; }
    // public string? departmentName { get; set; }
    // public string? candidateStatus { get; set; }
}
