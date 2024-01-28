using MediatR;

namespace E_Commerce.Application.Features.Quaries.Orders.GetByIdOrder
{
	public class GetByIdOrderQueryRequest : IRequest<GetByIdOrderQueryResponse>
	{
        public string Id { get; set; }
    }
}