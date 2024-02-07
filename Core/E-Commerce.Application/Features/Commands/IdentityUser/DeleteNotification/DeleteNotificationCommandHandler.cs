using E_Commerce.Application.Abstractions.Services.Notification;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommandRequest, DeleteNotificationCommandResponse>
	{
		private readonly INotificationService _notificationService;

		public DeleteNotificationCommandHandler(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		public async Task<DeleteNotificationCommandResponse> Handle(DeleteNotificationCommandRequest request, CancellationToken cancellationToken)
		{
			await _notificationService.DeleteNotification(request.Id);

			return new();
		}
	}
}
