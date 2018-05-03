namespace KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events
{
	using KitsorLab.BuildingBlocks.EventBus.Events;
	using System.Collections.Generic;

	public class CreateDeliveryIntegrationEvent : IntegrationEvent
	{
		public long NotificationKey { get; set; }
		public IList<long> SubscriptionKeys { get; set; }
	}
}
