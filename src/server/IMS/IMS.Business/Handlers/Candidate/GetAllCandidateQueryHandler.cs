using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class GetAllCandidateQueryHandler(IMapper mapper, IUnitOfWorks unitOfWork) : IRequestHandler<GetAllCandidateQuery, IEnumerable<CandidateViewModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWorks _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CandidateViewModel>> Handle(GetAllCandidateQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.CandidateRepository.GetQuery();
        var result = query.Include(c => c.Recruiter);
        return _mapper.Map<IEnumerable<CandidateViewModel>>(result);
    }
}
