
using IMS.Business.ViewModels.Department;
using MediatR;

namespace IMS.Business.Handlers
{
    public class GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentViewModel>>
    {
        
    }
}