
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class GetCandidateByIdQuery : IRequest<CandidateViewModel>
{
    public int Id { get; set; }
}
