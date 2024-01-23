using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Domain.Entities
{
	public class Order : BaseEntity
	{

		public Guid CustomerId { get; set; }
		public Basket Basket { get; set; }
		public string Description { get; set; }
		public Address Address { get; set; }
		public Customer Customer { get; set; }
		public ICollection<Product> Products { get; set; }

	}
}
