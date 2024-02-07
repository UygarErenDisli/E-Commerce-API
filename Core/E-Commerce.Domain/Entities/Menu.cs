using E_Commerce.Domain.Entities.Common;

namespace E_Commerce.Domain.Entities
{
	public class Menu : BaseEntity
	{
		public string Name { get; set; }
		public ICollection<Endpoint> Endpoints { get; set; }
	}
}
