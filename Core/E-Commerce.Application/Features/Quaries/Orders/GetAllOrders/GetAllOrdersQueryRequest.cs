using MediatR;

namespace E_Commerce.Application.Features.Quaries.Orders.GetAllOrders
{
	public class GetAllOrdersQueryRequest : IRequest<GetAllOrdersQueryResponse>
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
	}
}