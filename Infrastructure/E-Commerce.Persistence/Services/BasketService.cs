using E_Commerce.Application.Abstractions.Services.Baskets;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Services
{
	public class BasketService : IBasketService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IOrderReadRepository _orderReadRepository;
		private readonly IBasketWriteRepository _basketWriteRepository;
		private readonly IBasketReadRepository _basketReadRepository;
		private readonly IBasketItemReadRepository _basketItemReadRepository;
		private readonly IBasketItemWriteRepository _basketItemWriteRepository;

		public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketReadRepository basketReadRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
			_orderReadRepository = orderReadRepository;
			_basketWriteRepository = basketWriteRepository;
			_basketItemReadRepository = basketItemReadRepository;
			_basketItemWriteRepository = basketItemWriteRepository;
			_basketReadRepository = basketReadRepository;
		}

		public async Task<Basket> GetActiveBasketAsync()
		{
			var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
			if (!string.IsNullOrEmpty(username))
			{
				var user = await _userManager.Users
					.Include(u => u.Baskets)
					.FirstOrDefaultAsync(u => u.UserName == username);

				if (user == null)
				{
					throw new UserNotFoundException();
				}

				var userBaskets = from basket in user.Baskets
								  join order in _orderReadRepository.Table
								  on basket.Id equals order.Id into baskets
								  from order in baskets.DefaultIfEmpty()
								  select new
								  {
									  Basket = basket,
									  Order = order
								  };
				Basket? targetBasket = null;
				if (userBaskets.Any(b => b.Order is null))
				{
					targetBasket = userBaskets.FirstOrDefault(b => b.Order == null)?.Basket;
				}
				else
				{
					targetBasket = new();
					user.Baskets.Add(targetBasket);
				}
				await _basketWriteRepository.SaveAsync();
				return targetBasket!;

			}
			throw new UserNotFoundException();
		}

		public async Task AddItemToBasketAsync(string productId, int quantity)
		{
			var basket = await GetActiveBasketAsync();
			if (basket != null)
			{
				var basketItem = await _basketItemReadRepository
					.GetSingleAsync(item => item.BasketId == basket.Id && item.ProductId == Guid.Parse(productId));

				if (basketItem != null)
				{
					basketItem.Quantity += quantity;
				}
				else
				{
					await _basketItemWriteRepository.AddAsync(new()
					{
						BasketId = basket.Id,
						ProductId = Guid.Parse(productId),
						Quantity = quantity
					});
				}
				await _basketItemWriteRepository.SaveAsync();
			}
			else
			{
				throw new BasketNotFoundException();
			}
		}


		public async Task<List<BasketItem>> GetBasketItemsAsync()
		{
			var currentBasket = await GetActiveBasketAsync();
			if (currentBasket != null)
			{
				var includedBasket = await _basketReadRepository.Table
					.Include(b => b.BasketItems)
					.ThenInclude(b => b.Product)
					.FirstOrDefaultAsync(b => b.Id == currentBasket.Id);

				return includedBasket!.BasketItems.ToList();
			}
			else
			{
				throw new BasketNotFoundException();
			}
		}

		public async Task UpdateQuantityAsync(string basketItemId, int quantity)
		{
			var basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
			if (basketItemId != null)
			{
				basketItem.Quantity = quantity;
				await _basketItemWriteRepository.SaveAsync();
			}
			else
			{
				throw new BasketItemNotFoundException();
			}
		}
		public async Task RemoveItemAsync(string basketItemId)
		{

			var basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
			if (basketItem != null)
			{
				_basketItemWriteRepository.Remove(basketItem);
				await _basketItemWriteRepository.SaveAsync();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
