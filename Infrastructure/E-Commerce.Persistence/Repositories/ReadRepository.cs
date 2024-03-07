using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities.Common;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Persistence.Repositories
{
	public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
	{
		private readonly ECommerceDBContext _dBContext;

		public ReadRepository(ECommerceDBContext dBContext)
		{
			_dBContext = dBContext;
		}

		public DbSet<T> Table => _dBContext.Set<T>();

		public IQueryable<T> GetAll(bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
			{
				query.AsNoTracking();
			}
			return query;
		}
		public IQueryable<T> GetWhereAsync(Expression<Func<T, bool>> filter, bool tracking = true)
		{
			var query = Table.Where(filter);
			if (!tracking)
			{
				query.AsNoTracking();
			}
			return query.AsQueryable();
		}
		public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> filter, bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
			{
				query.AsNoTracking();
			}
			return await query.FirstOrDefaultAsync(filter);
		}
		public async Task<T?> GetByIdAsync(string id, bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
			{
				query.AsNoTracking();
			}
			return await query.FirstOrDefaultAsync(entity => entity.Id == Guid.Parse(id));
		}




	}
}
