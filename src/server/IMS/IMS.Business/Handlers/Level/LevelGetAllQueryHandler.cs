using System;
using AutoMapper;
using IMS.Business.ViewModels.Level;
using IMS.Data.UnitOfWorks;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Business.Handlers.Level;

public class LevelGetAllQueryHandler : IRequestHandler<LevelGetAllQuery, IEnumerable<LevelViewModel>>
{
    private IUnitOfWorks _unitOfWork;
    private IMapper _mapper;
    public LevelGetAllQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<LevelViewModel>> Handle(LevelGetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GenericRepository<IMS.Domain.Entities.Level>().GetAllAsync();
        return _mapper.Map<IEnumerable<LevelViewModel>>(result);
    }
}
