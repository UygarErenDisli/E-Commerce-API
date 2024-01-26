using E_Commerce.Application.DTOs.Order;

namespace E_Commerce.Application.Features.Quaries.Orders.GetAllOrders
{
	public class GetAllOrdersQueryResponse
	{
		public int TotalCount { get; set; }
		public List<ListOrderDTO>? Orders { get; set; }

	}
}