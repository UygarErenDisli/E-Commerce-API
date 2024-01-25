using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.SignaR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace E_Commerce.SignaR.HubServices
{
	public class OrderHubService : IOrderHubService
	{
		private readonly IHubContext<OrderHub> _hubContext;

		public OrderHubService(IHubContext<OrderHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task OrderCreatedMessageAsync(string message)
		{
			await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderCreatedMessage, message);
		}
	}
}
