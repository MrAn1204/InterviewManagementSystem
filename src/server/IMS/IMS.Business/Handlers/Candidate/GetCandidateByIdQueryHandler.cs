using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class GetCandidateByIdQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<GetCandidateByIdQuery, CandidateViewModel>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;
    public async Task<CandidateViewModel> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.CandidateRepository.GetQuery().Include(c=>c.Recruiter)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
            throw new ResourceNotFoundException("Candidate not found");

        return _mapper.Map<CandidateViewModel>(result);
    }
}
