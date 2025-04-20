using IMS.Business.ViewModels;

namespace IMS.Business.Handlers;

public class InterviewSearchQuery : BaseSearchQuery<InterviewViewModel>
{
    public int? InterviewerId { get; set; }
}
