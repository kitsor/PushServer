namespace KitsorLab.PushServer.API.Application.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using MediatR;

	public class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, Notification>
	{
		private readonly INotificationRepository _notificationRepository;

		/// <param name="notificationRepository"></param>
		public AddNotificationCommandHandler(INotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository 
				?? throw new ArgumentNullException(nameof(notificationRepository));
		}

		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<Notification> Handle(AddNotificationCommand command, CancellationToken cancellationToken)
		{
			Notification notification = new Notification(command.Notification.Title, command.Notification.Message,
				command.Notification.Url, command.Notification.IconUrl);

			notification = await _notificationRepository.AddAsync(notification);
			await _notificationRepository.UnitOfWork.SaveEntitiesAsync();
			return notification;
		}
	}
}
