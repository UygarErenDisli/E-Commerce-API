using E_Commerce.Application.Abstractions.Services;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetUserNotifications
{
	public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQueryRequest, List<GetUserNotificationsQueryResponse>>
	{
		private readonly INotificationService _notificationService;

		public GetUserNotificationsQueryHandler(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		public async Task<List<GetUserNotificationsQueryResponse>> Handle(GetUserNotificationsQueryRequest request, CancellationToken cancellationToken)
		{

			var response = await _notificationService.GetUserNotificationsAsync();
			return response.Select(d => new GetUserNotificationsQueryResponse()
			{
				Id = d.NotificationId,
				Message = d.Message,
				NotificationDate = d.NotificationDate.ToString("dd MMMM yyyy"),
				Subject = d.Subject
			}).ToList();


		}
	}
}
