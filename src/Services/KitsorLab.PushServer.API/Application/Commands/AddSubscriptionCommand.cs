namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using System.Net;

	public class AddSubscriptionCommand : IRequest<Subscription>
	{
		public IPAddress IPAddress { get; private set; }
		public SubscriptionRequest Subscription { get; private set; }

		public AddSubscriptionCommand(SubscriptionRequest subscription, IPAddress ipAddress)
		{
			Subscription = subscription;
			IPAddress = ipAddress;
		}
	}
}
