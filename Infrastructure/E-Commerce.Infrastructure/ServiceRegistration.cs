using E_Commerce.Application.Abstractions;
using E_Commerce.Application.Abstractions.Storage;
using E_Commerce.Infrastructure.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastractureServices(this IServiceCollection services)
		{
			services.AddScoped<IStorageService, StorageService>();
		}

		public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
		{
			services.AddScoped<IStorage, T>();
		}
	}
}
