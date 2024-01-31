using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Domain.Entities
{
	public class Order : BaseEntity
	{
		public string OrderCode { get; set; }
		public Basket Basket { get; set; }
		public string Description { get; set; }
		public Address Address { get; set; }
		public CompletedOrder CompletedOrder { get; set; }
	}
}
