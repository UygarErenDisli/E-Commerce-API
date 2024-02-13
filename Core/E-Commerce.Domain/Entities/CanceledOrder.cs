using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Domain.Entities
{
	public class CanceledOrder : BaseEntity
	{
		public Guid OrderId { get; set; }
		public Order Order { get; set; }
		public string Reason { get; set; }
	}
}
