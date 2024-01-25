using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Abstractions.Services.Baskets
{
	public interface IBasketService
	{
		public Task<List<BasketItem>> GetBasketItemsAsync();
		public Task AddItemToBasketAsync(string productId, int quantity);
		public Task UpdateQuantityAsync(string basketItemId, int quantity);
		public Task RemoveItemAsync(string basketItemId);
		public Task<Basket> GetActiveBasketAsync();

	}
}
