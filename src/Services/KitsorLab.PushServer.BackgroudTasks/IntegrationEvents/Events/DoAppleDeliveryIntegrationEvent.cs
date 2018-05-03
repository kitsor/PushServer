namespace KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events
{
	using KitsorLab.BuildingBlocks.EventBus.Events;

	public class DoAppleDeliveryIntegrationEvent : IntegrationEvent
	{
		public long DeliveryKey { get; set; }
	}
}
