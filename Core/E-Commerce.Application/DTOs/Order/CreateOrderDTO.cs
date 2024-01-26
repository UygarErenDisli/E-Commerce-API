namespace E_Commerce.Application.DTOs.Order
{
	public class CreateOrderDTO
	{
		public Guid BasketId { get; set; }
		public string Description { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}
