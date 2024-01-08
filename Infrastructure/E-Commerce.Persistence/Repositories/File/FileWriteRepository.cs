using E_Commerce.Application.Repositories;
using E_Commerce.Persistence.Context;
using File = E_Commerce.Domain.Entities.File;

namespace E_Commerce.Persistence.Repositories
{
	internal class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
	{
		public FileWriteRepository(ECommerceDBContext dBContext) : base(dBContext)
		{
		}
	}
}
