using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Business.Handlers
{
	public class BaseDeleteByIdCommand<T> : IRequest<T>
	{
		public Guid Id { get; set; }

		public bool IsHardDelete { get; set; } = false;
	}
}