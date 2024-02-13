using E_Commerce.Application.DTOs.Notification;

namespace E_Commerce.Application.Abstractions.Services.Notification
{
	public interface INotificationService
	{
		Task CreateOrderCompletedNotificationAsync(OrderCompletedNotificationDTO notification);
		Task CreateOrderCanceledNotificationAsync(OrderCanceledNotificationDTO notification);
		Task DeleteNotification(string notificationId);
		Task<List<UserNotificationDTO>> GetUserNotificationsAsync();
	}
}

