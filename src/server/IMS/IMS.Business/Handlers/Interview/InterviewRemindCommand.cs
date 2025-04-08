using MediatR;

namespace IMS.Business.Handlers;

public class InterviewRemindCommand : IRequest<bool>
{
    public required string Email { get; set; }

    public required int InterviewId { get; set; }

    public required string InterviewLink { get; set; }
}
