namespace KitsorLab.PushServer.API.Application.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.Kernel.Extensions;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;

	public class AddSubscriptionCommandHandler : IRequestHandler<AddSubscriptionCommand, Subscription>
	{
		private readonly ISubscriptionRepository _subscriptionRepository;

		/// <param name="subscriptionRepository"></param>
		public AddSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
		{
			_subscriptionRepository = subscriptionRepository 
				?? throw new ArgumentException(nameof(subscriptionRepository));
		}

		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<Subscription> Handle(AddSubscriptionCommand command, CancellationToken cancellationToken)
		{
			Subscription subscription;

			if (!string.IsNullOrEmpty(command.Subscription.Endpoint))
			{
				string userId = command.Subscription.Endpoint.GetMD5HashString(true);
				subscription = new Subscription(userId, command.Subscription.Endpoint, command.Subscription.PublicKey,
						command.Subscription.Auth);
			}
			else
			{
				string userId = command.Subscription.DeviceToken.GetMD5HashString(true);
				subscription = new Subscription(userId, command.Subscription.DeviceToken);
			}
	
			Subscription existed = await _subscriptionRepository.GetByUserIdAsync(subscription.UserId, false);
			if (existed != null)
			{
				existed.SetUpdatedOnNow();
				subscription = existed;
			}

			// IPv4 only
			if (command.IPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
			{
				subscription.SetIpAddress(command.IPAddress);
			}

			subscription = await _subscriptionRepository.AddAsync(subscription);
			await _subscriptionRepository.UnitOfWork.SaveEntitiesAsync();

			return subscription;
		}
	}
}
