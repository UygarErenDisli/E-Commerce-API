using E_Commerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace E_Commerce.Application.Repositories
{
	public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
	{
		IQueryable<T> GetAll(bool tracking = true);
		IQueryable<T> GetWhereAsync(Expression<Func<T, bool>> filter, bool tracking = true);
		Task<T> GetSingleAsync(Expression<Func<T, bool>> filter, bool tracking = true);
		Task<T> GetByIdAsync(string id, bool tracking = true);
	}
}
