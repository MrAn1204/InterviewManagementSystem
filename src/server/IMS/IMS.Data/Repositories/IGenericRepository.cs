
using System.Linq.Expressions;


namespace IMS.Data.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		IEnumerable<T> GetAll();
		IQueryable<T> GetAllQuery();
		Task<IEnumerable<T>> GetAllAsync();

		T? GetById(int id);

		Task<T?> GetByIdAsync(int id);

		void Add(T entity);

		void Update(T entity);

		void Delete(int id);

		void Delete(T entity, bool isHardDelete = false);

		void Delete(Expression<Func<T, bool>> where, bool isHardDelete = false);

		IQueryable<T> GetQuery();

		IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

		IQueryable<T> Get(Expression<Func<T, bool>>? filter = null,
						  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
						  string includeProperties = "");
		void AddRange(T[] entities);
	}
}