namespace E_Commerce.Application.Features.Quaries.Baskets.GetAllBasketItems
{
	public class GetAllBasketItemsQueryResponse
	{
		public string BasketItemId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}