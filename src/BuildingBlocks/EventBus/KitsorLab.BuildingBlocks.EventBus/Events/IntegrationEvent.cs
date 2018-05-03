namespace KitsorLab.BuildingBlocks.EventBus.Events
{
	using System;

	public class IntegrationEvent
	{
		public Guid Id { get; }
		public DateTime CreationDate { get; }

		public IntegrationEvent()
		{
			Id = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}
	}
}
