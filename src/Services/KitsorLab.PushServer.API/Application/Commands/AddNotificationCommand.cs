namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using MediatR;

	public class AddNotificationCommand : IRequest<Notification>
	{
		public NotificationRequest Notification { get; private set; }

		public AddNotificationCommand(NotificationRequest notification)
		{
			Notification = notification;
		}
	}
}
