namespace KitsorLab.PushServer.API.Application.IntegrationEvents.Events
{
	using KitsorLab.BuildingBlocks.EventBus.Events;

	public class AddDeliveryIntegrationEvent : IntegrationEvent
	{
		public long NotificationKey { get; set; }
		public long SubscriptionKey { get; set; }
	}
}
