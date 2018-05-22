namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using MediatR;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	public class GetNotificationListCommandHandler : IRequestHandler<GetNotificationListCommand, PaginatedResponse<IList<Notification>>>
	{
		private readonly INotificationRepository _notificationRepository;

		/// <param name="subscriptionRepository"></param>
		public GetNotificationListCommandHandler(INotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository
				?? throw new ArgumentException(nameof(notificationRepository));
		}

		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<PaginatedResponse<IList<Notification>>> Handle(GetNotificationListCommand command, CancellationToken cancellationToken)
		{
			NotificationListRequest request = command.Request;
			IList<Notification> list =
				await _notificationRepository.GetListAsync(request.GetLimit(), request.GetOffset(), x => x.CreatedOn, true);
			long total = await _notificationRepository.GetListTotalAsync(x => x.CreatedOn, true);

			int totalPages = (int)Math.Ceiling((decimal)total / request.EntriesPerPage);
			var result = new PaginatedResponse<IList<Notification>>(list, total, request.Page, totalPages, request.GetLimit());
			return result;
		}
	}
}
