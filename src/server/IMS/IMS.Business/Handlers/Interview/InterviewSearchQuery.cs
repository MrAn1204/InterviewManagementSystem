using IMS.Business.ViewModels;
using IMS.Domain;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewSearchQuery : BaseSearchQuery<InterviewViewModel>
{
    public int? InterviewerId { get; set; }
}
