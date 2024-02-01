using E_Commerce.Domain.Entities.Common;
using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Domain.Entities
{
	public class Notification : BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; } = null!;
		public string Message { get; set; }
		public string Subject { get; set; }

	}
}
