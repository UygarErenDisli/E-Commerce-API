using E_Commerce.Application.Repositories;
using E_Commerce.Persistence.Context;
using E_Commerce.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Persistence.Extentions
{
	public static class ServicesRegistration
	{
		public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ECommerceDBContext>(options => options.UseNpgsql(connectionString));
			services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
			services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

			services.AddScoped<IOrderReadRepository, OrderReadRepository>();
			services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

			services.AddScoped<IProductReadRepository, ProductReadRepository>();
			services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

			services.AddScoped<IFileReadRepository, FileReadRepository>();
			services.AddScoped<IFileWriteRepository, FileWriteRepository>();

			services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
			services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

			services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
			services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();
		}
	}
}
