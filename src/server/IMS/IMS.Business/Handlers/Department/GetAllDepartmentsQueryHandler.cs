using AutoMapper;
using IMS.Business.ViewModels.Department;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Departments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentViewModel>>
{
    private IUnitOfWorks _unitOfWork;
    private IMapper _mapper;
    public GetAllDepartmentsQueryHandler(IUnitOfWorks unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<DepartmentViewModel>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GenericRepository<IMS.Domain.Entities.Department>().GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentViewModel>>(result);
    }
}