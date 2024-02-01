namespace E_Commerce.Application.DTOs.Notification
{
	public class UserNotificationDTO
	{
		public string NotificationId { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public DateTime NotificationDate { get; set; }
	}
}
