using E_Commerce.Domain.Entities.Common;
using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Domain.Entities
{
	public class Basket : BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public Order Order { get; set; }
		public ICollection<BasketItem> BasketItems { get; set; }
	}
}
