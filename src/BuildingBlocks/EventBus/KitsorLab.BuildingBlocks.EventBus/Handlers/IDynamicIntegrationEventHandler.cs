namespace KitsorLab.BuildingBlocks.EventBus.Handlers
{
	using System.Threading.Tasks;

	public interface IDynamicIntegrationEventHandler
	{
		Task Handle(dynamic eventData);
	}
}
