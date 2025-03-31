using AutoMapper;
using IMS.Business.ViewModels.Benefit;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers.Benefit;

public class BenefitGetAllQueryHandler : IRequestHandler<BenefitGetAllQuery, IEnumerable<BenefitViewModel>>
{
    private IUnitOfWorks _unitOfWork;
    private IMapper _mapper;
    public BenefitGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BenefitViewModel>> Handle(BenefitGetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GenericRepository<IMS.Domain.Entities.Benefit>().GetAllAsync();
        return _mapper.Map<IEnumerable<BenefitViewModel>>(result);
    }
}
