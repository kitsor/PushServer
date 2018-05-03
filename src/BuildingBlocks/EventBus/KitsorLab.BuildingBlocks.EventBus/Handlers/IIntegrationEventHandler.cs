namespace KitsorLab.BuildingBlocks.EventBus.Handlers
{
	using KitsorLab.BuildingBlocks.EventBus.Events;
	using System.Threading.Tasks;

	public interface IIntegrationEventHandler
	{
	}

	public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
				where TIntegrationEvent : IntegrationEvent
	{
		Task Handle(TIntegrationEvent @event);
	}
}
