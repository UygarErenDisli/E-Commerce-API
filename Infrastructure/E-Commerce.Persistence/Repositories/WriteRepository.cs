using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities.Common;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace E_Commerce.Persistence.Repositories
{
	public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
	{
		private readonly ECommerceDBContext _dBContext;

		public WriteRepository(ECommerceDBContext dBContext)
		{
			_dBContext = dBContext;
		}

		public DbSet<T> Table => _dBContext.Set<T>();

		public async Task<bool> AddAsync(T model)
		{
			EntityEntry<T> entity = await Table.AddAsync(model);
			return entity.State == EntityState.Added;
		}

		public async Task<bool> AddRangeAsync(List<T> models)
		{
			await Table.AddRangeAsync(models);
			return true;
		}

		public bool Remove(T model)
		{
			EntityEntry<T> entity = Table.Remove(model);
			return entity.State == EntityState.Deleted;

		}

		public async Task<bool> RemoveAsync(string id)
		{
			var model = await Table.FirstOrDefaultAsync(entity => entity.Id == Guid.Parse(id));
			return Remove(model);
		}

		public bool RemoveRange(List<T> models)
		{
			Table.RemoveRange(models);
			return true;
		}

		public async Task<int> SaveAsync() => await _dBContext.SaveChangesAsync();

		public bool Update(T model)
		{
			EntityEntry<T> entity = Table.Update(model);
			return entity.State == EntityState.Modified;
		}
	}

}
