using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;

namespace E_Commerce.Persistence.Repositories
{
	public class NotificationWriteRepository : WriteRepository<Notification>, INotificationWriteRepository
	{
		public NotificationWriteRepository(ECommerceDBContext dBContext) : base(dBContext)
		{
		}
	}
}
