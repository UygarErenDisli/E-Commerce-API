using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Domain.Entities.Identity
{
	public class AppRole : IdentityRole<string>
	{
		public ICollection<Endpoint> Endpoints { get; set; }
	}
}
