using MediatR;


namespace IMS.Business.Handlers
{
	public class BaseGetAllQuery<T> : IRequest<IEnumerable<T>> where T : class
	{
	}
}