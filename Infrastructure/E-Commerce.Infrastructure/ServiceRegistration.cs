using E_Commerce.Application.Abstractions.Services.Configurations;
using E_Commerce.Application.Abstractions.Storage;
using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Infrastructure.Services.Configurations;
using E_Commerce.Infrastructure.Services.Storage;
using E_Commerce.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastractureServices(this IServiceCollection services)
		{
			services.AddScoped<IStorageService, StorageService>();
			services.AddScoped<ITokenHandler, TokenHandler>();
			services.AddScoped<IApplicationService, ApplicationService>();
		}

		public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
		{
			services.AddScoped<IStorage, T>();
		}
	}
}
