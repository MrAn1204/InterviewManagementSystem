using AutoMapper;
using IMS.Data.UnitOfWorks;


namespace IMS.Business.Handlers
{
	public class BaseHandler(IUnitOfWorks unitOfWork, IMapper mapper)
	{
		protected readonly IUnitOfWorks _unitOfWork = unitOfWork;

		protected readonly IMapper _mapper = mapper;
	}
}