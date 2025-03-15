using IMS.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Business.Handlers
{
	public class BaseSearchQuery<T> :
	IRequest<PaginatedResult<T>> where T : class
	{
		public string? Keyword { get; set; }

		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 10;

		public string? OrderBy { get; set; } = "CreatedAt";

		public OrderDirection OrderDirection { get; set; } = OrderDirection.ASC;
	}

	public class MasterDataSearchQuery<T> : BaseSearchQuery<T> where T : class
	{
		public bool? IncludeInactive { get; set; }
	}

	public enum OrderDirection
	{
		ASC,
		DESC
	}
}