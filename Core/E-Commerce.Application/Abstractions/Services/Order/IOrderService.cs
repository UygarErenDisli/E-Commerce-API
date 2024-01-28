using E_Commerce.Application.DTOs.Order;

namespace E_Commerce.Application.Abstractions.Services.Order
{
	public interface IOrderService
	{
		int GetTotalOrdersCount();
		Task CreateOrderAsync(CreateOrderDTO createOrder);
		Task<List<ListOrderDTO>> GetAllOrdersAsync(int pageIndex, int pageSize);
		Task<SingleDetailedOrderDTO> GetOrderByIdAsync(string id);

	}
}
