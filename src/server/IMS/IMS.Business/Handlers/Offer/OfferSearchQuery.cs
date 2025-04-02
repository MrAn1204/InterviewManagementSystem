using System;
using IMS.Business.ViewModels;

namespace IMS.Business.Handlers;

public class OfferSearchQuery : BaseSearchQuery<OfferViewModel>
{
    public string? departmentName { get; set; }
    public string? candidateStatus { get; set; }
}
