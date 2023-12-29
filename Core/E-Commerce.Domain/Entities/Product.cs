using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Domain.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
