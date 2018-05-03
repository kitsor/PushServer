namespace KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Handlers
{
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events;
	using KitsorLab.PushServer.BackgroudTasks.Tasks.Queue;
	using System;
	using System.Threading.Tasks;

	public class DoAppleDeliveryIntegrationEventHandler : IIntegrationEventHandler<DoAppleDeliveryIntegrationEvent>
	{
		private readonly int _defaultMaxQueue = 10;
		private readonly int _defaultSleepInSeconds = 5;
		private readonly IAppleDeliveryItemsQueue _queue;

		public DoAppleDeliveryIntegrationEventHandler(IAppleDeliveryItemsQueue queue)
		{
			_queue = queue ?? throw new ArgumentNullException(nameof(queue));
		}

		/// <param name="event"></param>
		/// <returns></returns>
		public async Task Handle(DoAppleDeliveryIntegrationEvent @event)
		{
			while (_queue.Count > _defaultMaxQueue)
			{
				await Task.Delay(TimeSpan.FromSeconds(_defaultSleepInSeconds));
			}

			_queue.QueueItem(@event.DeliveryKey);
		}
	}
}
