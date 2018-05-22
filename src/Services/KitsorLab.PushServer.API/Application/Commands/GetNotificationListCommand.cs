namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using MediatR;
	using System.Collections.Generic;

	public class GetNotificationListCommand : IRequest<PaginatedResponse<IList<Notification>>>
	{
		public NotificationListRequest Request { get; private set; }

		public GetNotificationListCommand(NotificationListRequest model)
		{
			Request = model;
		}
	}
}
