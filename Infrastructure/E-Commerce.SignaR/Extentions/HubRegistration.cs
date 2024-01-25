using E_Commerce.SignaR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace E_Commerce.SignaR.Extentions
{
	public static class HubRegistration
	{
		public static void MapHubs(this WebApplication application)
		{
			application.MapHub<ProductHub>("/products/hub");
			application.MapHub<OrderHub>("/orders/hub");
		}
	}
}
