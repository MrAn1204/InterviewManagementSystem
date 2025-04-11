using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateCountByWeekQuery : IRequest<IEnumerable<CandidateCountByWeekViewModel>>
{
    public int Month { get; set; }
    public int Year { get; set; }
}
