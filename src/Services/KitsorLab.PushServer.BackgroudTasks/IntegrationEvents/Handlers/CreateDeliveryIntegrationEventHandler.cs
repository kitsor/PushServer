namespace KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Handlers
{
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class CreateDeliveryIntegrationEventHandler : IIntegrationEventHandler<CreateDeliveryIntegrationEvent>
	{
		private readonly IDeliveryRepository _deliveryRepository;

		/// <param name="deliveryRepository"></param>
		public CreateDeliveryIntegrationEventHandler(IDeliveryRepository deliveryRepository)
		{
			_deliveryRepository = deliveryRepository;
		}

		/// <param name="event"></param>
		/// <returns></returns>
		public async Task Handle(CreateDeliveryIntegrationEvent @event)
		{
			IList<Delivery> deliveries = @event.SubscriptionKeys
				.Select(x => new Delivery(@event.NotificationKey, x, @event.ScheduledOn))
				.ToList();

			await _deliveryRepository.SaveRange(deliveries);
		}
	}
}
