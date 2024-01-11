using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace E_Commerce.Application.Extentions
{
	public static class ServiceRegistration
	{

		public static void AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(configuration =>
			{
				configuration.RegisterServicesFromAssembly(assembly: Assembly.GetExecutingAssembly());
			});
		}
	}
}
