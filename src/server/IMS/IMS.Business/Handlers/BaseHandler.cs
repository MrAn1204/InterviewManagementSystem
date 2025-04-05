using AutoMapper;
using IMS.Data.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Business.Handlers
{
	public class BaseHandler(IUnitOfWorks unitOfWork, IMapper mapper)
	{
		protected readonly IUnitOfWorks _unitOfWork = unitOfWork;

		protected readonly IMapper _mapper = mapper;
	}
}