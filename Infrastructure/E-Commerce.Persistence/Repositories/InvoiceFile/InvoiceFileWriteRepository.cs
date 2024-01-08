using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;

namespace E_Commerce.Persistence.Repositories
{
	public class InvoiceFileWriteRepository : WriteRepository<InvoiceFile>, IInvoiceFileWriteRepository
	{
		public InvoiceFileWriteRepository(ECommerceDBContext dBContext) : base(dBContext)
		{
		}
	}
}
