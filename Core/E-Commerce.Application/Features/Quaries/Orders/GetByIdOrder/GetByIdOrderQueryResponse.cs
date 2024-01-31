using E_Commerce.Application.DTOs.Address;
using E_Commerce.Application.DTOs.BasketItem;

namespace E_Commerce.Application.Features.Quaries.Orders.GetByIdOrder
{
	public class GetByIdOrderQueryResponse
	{
		public string Id { get; set; }
		public string OrderCode { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public decimal TotalPrice { get; set; }
		public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
		public AddressDTO Address { get; set; }
		public List<BasketItemDTO> BasketItems { get; set; }
	}
}
