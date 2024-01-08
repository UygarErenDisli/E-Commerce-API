using E_Commerce.Application.Repositories;
using E_Commerce.Persistence.Context;
using File = E_Commerce.Domain.Entities.File;

namespace E_Commerce.Persistence.Repositories
{
	public class FileReadRepository : ReadRepository<File>, IFileReadRepository
	{
		public FileReadRepository(ECommerceDBContext dBContext) : base(dBContext)
		{
		}
	}
}
