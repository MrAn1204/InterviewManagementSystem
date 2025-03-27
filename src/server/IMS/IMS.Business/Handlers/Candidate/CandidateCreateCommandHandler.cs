using System;
using MediatR;

namespace IMS.Business.Handlers.Candidate;

public class CandidateCreateCommandHandler : IRequestHandler<CandidateCreateCommand, bool>
{
    public Task<bool> Handle(CandidateCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
