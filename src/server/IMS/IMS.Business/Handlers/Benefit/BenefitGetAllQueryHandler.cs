using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class BenefitGetAllQueryHandler : BaseHandler,
    IRequestHandler<BenefitGetAllQuery, IEnumerable<BenefitViewModel>>
{
    public BenefitGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<BenefitViewModel>> Handle(BenefitGetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GenericRepository<IMS.Domain.Entities.Benefit>().GetAllAsync();
        return _mapper.Map<IEnumerable<BenefitViewModel>>(result);
    }
}
