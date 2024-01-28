namespace E_Commerce.Application.DTOs.BasketItem
{
	public class BasketItemDTO
	{
		public string ProductId { get; set; }
		public int Quantity { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
	}
}
