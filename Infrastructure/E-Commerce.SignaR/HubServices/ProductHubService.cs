using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.SignaR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace E_Commerce.SignaR.HubServices
{
	public class ProductHubService : IProductHubService
	{
		private readonly IHubContext<ProductHub> _hubContext;

		public ProductHubService(IHubContext<ProductHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task ProductAddedMessageAsync(string message)
		{
			await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
		}
	}
}
