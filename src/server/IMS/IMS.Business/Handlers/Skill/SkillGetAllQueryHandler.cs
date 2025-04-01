using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class SkillGetAllQueryHandler : BaseHandler,
    IRequestHandler<SkillGetAllQuery, IEnumerable<SkillViewModel>>
{
    public SkillGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<SkillViewModel>> Handle(SkillGetAllQuery request, CancellationToken cancellationToken)
    {
        var result=await _unitOfWork.GenericRepository<IMS.Domain.Entities.Skill>().GetAllAsync();
        return _mapper.Map<IEnumerable<SkillViewModel>>(result);
    }
}
