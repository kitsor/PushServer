namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using System.Collections.Generic;

	public class GetSubscriptionListCommand : IRequest<PaginatedResponse<IList<Subscription>>>
	{
		public SubscriptionListRequest Request { get; private set; }

		public GetSubscriptionListCommand(SubscriptionListRequest model)
		{
			Request = model;
		}
	}
}
