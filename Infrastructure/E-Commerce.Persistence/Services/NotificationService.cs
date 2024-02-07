using E_Commerce.Application.Abstractions.Services.Notification;
using E_Commerce.Application.DTOs.Notification;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace E_Commerce.Persistence.Services
{
    public class NotificationService : INotificationService
	{
		private readonly INotificationReadRepository _notificationReadRepository;
		private readonly INotificationWriteRepository _notificationWriteRepository;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public NotificationService(INotificationReadRepository notificationReadRepository, INotificationWriteRepository notificationWriteRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_notificationReadRepository = notificationReadRepository;
			_notificationWriteRepository = notificationWriteRepository;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task CreateOrderCompletedNotificationAsync(OrderCompletedNotificationDTO model)
		{
			var subject = "Your Order Has Been Shipped";
			var message = CreateNotificationMessage(model.UserName, model.OrderCode, model.OrderDate);

			if (!string.IsNullOrEmpty(model.UserId))
			{
				await _notificationWriteRepository.AddAsync(new()
				{
					Id = Guid.NewGuid(),
					Message = message,
					UserId = model.UserId,
					Subject = subject
				});
				await _notificationWriteRepository.SaveAsync();
			}
			else
			{
				throw new InvalidUserIdException("Invalid User Id");
			}
		}


		public async Task<List<UserNotificationDTO>> GetUserNotificationsAsync()
		{
			var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name!;
			if (!string.IsNullOrEmpty(username))
			{
				var user = await _userManager.FindByNameAsync(username);

				if (user != null)
				{
					var notifications = await _notificationReadRepository.GetWhereAsync(n => n.UserId == user.Id.ToString()).ToListAsync();
					return notifications.Select(notification => new UserNotificationDTO()
					{
						NotificationId = notification.Id.ToString(),
						Message = notification.Message,
						NotificationDate = notification.CreatedDate,
						Subject = notification.Subject
					}).ToList();
				}
				else
				{
					throw new UserNotFoundException("User not Found");
				}
			}
			else
			{
				throw new Exception("An Error Occurred while getting notifications");
			}

		}

		public async Task DeleteNotification(string notificationId)
		{
			if (!string.IsNullOrEmpty(notificationId))
			{
				var notification = await _notificationReadRepository.GetByIdAsync(notificationId);
				if (notification != null)
				{
					_notificationWriteRepository.Remove(notification);
					await _notificationWriteRepository.SaveAsync();
				}
				else
				{
					throw new ArgumentException("Notification Couldn't Found");
				}
			}
			else
			{
				throw new ArgumentException("Invalid NotificationId");
			}
		}

		private string CreateNotificationMessage(string userName, string orderCode, DateTime orderDate)
		{
			StringBuilder builder = new();
			builder.AppendLine($"Hello {userName}");
			builder.AppendLine();
			builder.AppendLine($"Your order with order code [{orderCode}] has been shipped!");
			builder.AppendLine($"The order you placed on [{orderDate.ToString("dd MMMM yyyy")}] is currently in the shipping process.");
			builder.AppendLine();
			builder.AppendLine("Have a great day!");

			return builder.ToString();
		}


	}
}
