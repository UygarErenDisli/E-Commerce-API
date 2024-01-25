using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.SignaR.HubServices;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.SignaR.Extentions
{
	public static class ServiceRegistration
	{
		public static void AddSignalRServices(this IServiceCollection services)
		{
			services.AddSignalR();
			services.AddTransient<IProductHubService, ProductHubService>();

			services.AddTransient<IOrderHubService, OrderHubService>();
		}

	}
}
