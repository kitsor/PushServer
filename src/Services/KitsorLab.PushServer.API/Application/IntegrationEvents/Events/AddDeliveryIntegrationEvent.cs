namespace KitsorLab.PushServer.API.Application.IntegrationEvents.Events
{
	using KitsorLab.BuildingBlocks.EventBus.Events;
	using System;

	public class AddDeliveryIntegrationEvent : IntegrationEvent
	{
		public long NotificationKey { get; set; }
		public long SubscriptionKey { get; set; }
		public DateTime? ScheduledOn { get; set; }
	}
}
