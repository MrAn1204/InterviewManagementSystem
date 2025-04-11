using System;
using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class JobCountByWeekQuery: IRequest<IEnumerable<JobCountByWeekViewModel>>
{
    public int Month { get; set; }
    public int Year { get; set; }
}
