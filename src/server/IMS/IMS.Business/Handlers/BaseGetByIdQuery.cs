using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Business.Handlers
{
	public class BaseGetByIdQuery<T> : IRequest<T> where T : class
	{
		public Guid Id { get; set; }
	}
}