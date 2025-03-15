using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Business.Handlers
{
	public class BaseGetAllQuery<T> : IRequest<IEnumerable<T>> where T : class
	{
	}
}