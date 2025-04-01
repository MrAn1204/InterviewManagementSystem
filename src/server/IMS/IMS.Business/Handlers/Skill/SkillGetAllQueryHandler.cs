
using AutoMapper;
using IMS.Business.ViewModels.Skill;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers.Skill;

public class SkillGetAllQueryHandler : IRequestHandler<SkillGetAllQuery, IEnumerable<SkillViewModel>>
{
    private IUnitOfWorks _unitOfWork;
    private IMapper _mapper;
    public SkillGetAllQueryHandler(IUnitOfWorks unitOfWork,IMapper mapper)
    {
        _unitOfWork=unitOfWork;
        _mapper=mapper;
    }

    public async Task<IEnumerable<SkillViewModel>> Handle(SkillGetAllQuery request, CancellationToken cancellationToken)
    {
        var result=await _unitOfWork.GenericRepository<IMS.Domain.Entities.Skill>().GetAllAsync();
        return _mapper.Map<IEnumerable<SkillViewModel>>(result);
    }
}
