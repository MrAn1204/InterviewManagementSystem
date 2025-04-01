using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Business.Handlers;

public class LevelGetAllQueryHandler : BaseHandler,
    IRequestHandler<LevelGetAllQuery, IEnumerable<LevelViewModel>>
{
    public LevelGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<LevelViewModel>> Handle(LevelGetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GenericRepository<Level>().GetAllAsync();
        return _mapper.Map<IEnumerable<LevelViewModel>>(result);
    }
}
