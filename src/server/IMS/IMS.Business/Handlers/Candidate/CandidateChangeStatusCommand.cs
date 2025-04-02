using IMS.Domain;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateChangeStatusCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Status { get; set; } = CandidateStatus.Open.ToString();
}
