using System;
using MediatR;


namespace IMS.Business.Handlers;

public class CandidateDeleteCommand : IRequest<bool>
{
    public int Id { get; set; }
}
