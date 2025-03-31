using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers.Job;

public class JobCreateUpdateCommandHandler : BaseHandler,
    IRequestHandler<JobCreateAndUpdateCommand, JobViewModel>
{
    public JobCreateUpdateCommandHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public Task<JobViewModel> Handle(JobCreateAndUpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
