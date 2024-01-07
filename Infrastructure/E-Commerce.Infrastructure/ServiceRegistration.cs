using E_Commerce.Application.Services;
using E_Commerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastractureServices(this IServiceCollection services)
		{
			services.AddScoped<IFileService, FileService>();
		}
	}
}
