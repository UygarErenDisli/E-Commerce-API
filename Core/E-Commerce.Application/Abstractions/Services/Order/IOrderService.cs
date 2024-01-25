using E_Commerce.Application.DTOs.Order;

namespace E_Commerce.Application.Abstractions.Services.Order
{
	public interface IOrderService
	{
		Task CreateOrderAsync(CreateOrderDTO createOrder);
	}
}
