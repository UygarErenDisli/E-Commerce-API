using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Application.Repositories
{
	public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
	{
		Task<bool> AddAsync(T model);
		Task<bool> AddRangeAsync(List<T> models);
		Task<bool> RemoveAsync(string id);
		bool RemoveRange(List<T> models);
		bool Remove(T model);
		bool Update(T model);
		Task<int> SaveAsync();
	}
}
