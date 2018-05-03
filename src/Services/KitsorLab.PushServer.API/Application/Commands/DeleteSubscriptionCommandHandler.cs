namespace KitsorLab.PushServer.API.Application.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;

	public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, bool>
	{
		private readonly ISubscriptionRepository _subscriptionRepository;

		/// <param name="subscriptionRepository"></param>
		public DeleteSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
		{
			_subscriptionRepository = subscriptionRepository
				?? throw new ArgumentException(nameof(subscriptionRepository));
		}

		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<bool> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
		{
			Subscription subscription = await _subscriptionRepository.GetByUserIdAsync(command.UserId, false);
			if (subscription != null)
			{
				await _subscriptionRepository.DeleteAsync(subscription);
				await _subscriptionRepository.UnitOfWork.SaveEntitiesAsync();
			}

			return true;
		}
	}
}
