using E_Commerce.Application.Abstractions.Services;
using E_Commerce.Application.Abstractions.Services.Authentication;
using E_Commerce.Application.Abstractions.Services.Baskets;
using E_Commerce.Application.Abstractions.Services.Order;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Persistence.Context;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Persistence.Extentions
{
	public static class ServicesRegistration
	{
		public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ECommerceDBContext>(options => options.UseNpgsql(connectionString));

			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.Password.RequiredLength = 3;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireDigit = false;
			})
				.AddEntityFrameworkStores<ECommerceDBContext>();

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

			services.AddScoped<IBasketReadRepository, BasketReadRepository>();
			services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

			services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
			services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

			services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
			services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();

			services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
			services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();

			services.AddScoped<IUserService, UserService>();

			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IInternalAuthentication, AuthService>();
			services.AddScoped<IExternalAuthentication, AuthService>();


			services.AddScoped<IBasketService, BasketService>();

			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<INotificationService, NotificationService>();
		}
	}
}
