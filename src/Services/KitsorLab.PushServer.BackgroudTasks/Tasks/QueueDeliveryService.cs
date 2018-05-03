namespace KitsorLab.PushServer.BackgroudTasks.Tasks
{
	using KitsorLab.BuildingBlocks.EventBus;
	using KitsorLab.PushServer.BackgroudTasks.Configuration;
	using KitsorLab.PushServer.BackgroudTasks.IntegrationEvents.Events;
	using KitsorLab.PushServer.BackgroudTasks.Queries;
	using KitsorLab.PushServer.BackgroudTasks.Tasks.Base;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	public class QueueDeliveryService : BackgroundService
	{
		private readonly IDeliveryQueries _deliveryQueries;
		private readonly QueueDeliveryTaskSettings _settings;
		private readonly IEventBus _eventBus;
		private readonly ILogger<QueueDeliveryService> _logger;

		public QueueDeliveryService(
			IDeliveryQueries deliveryQueries,
			IOptions<QueueDeliveryTaskSettings> settings,
			IEventBus eventBus,
			ILogger<QueueDeliveryService> logger)
		{
			_deliveryQueries = deliveryQueries ?? throw new ArgumentNullException(nameof(deliveryQueries));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <param name="stoppingToken"></param>
		/// <returns></returns>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogDebug($"[TASK] QueueDeliveryService is starting.");
			stoppingToken.Register(() => _logger.LogDebug($"[TASK] QueueDeliveryService background task is stopping."));

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogDebug($"[TASK] QueueDeliveryService background task is doing background work.");

				int numEntities = 0;
				try
				{
					numEntities = await DoWork();
				}
				catch (Exception ex)
				{
					_logger.LogError($"[TASK] QueueDeliveryService got Exception: {ex.Message}, Trace: {ex.StackTrace}");
				}

				if (numEntities == 0)
				{
					await Task.Delay(TimeSpan.FromSeconds(_settings.CheckUpdateTime), stoppingToken);
				}
			}

			_logger.LogDebug($"[TASK] QueueDeliveryService background task is stopping.");

			await Task.CompletedTask;
		}

		/// <returns></returns>
		private async Task<int> DoWork()
		{
			IDictionary<long, SubscriptionType> deliveries = await _deliveryQueries.GetNewDeliveriesAndSetInQueue();

			foreach (var key in deliveries)
			{
				switch(key.Value)
				{
					case SubscriptionType.W3C:
						_eventBus.Publish(new DoDeliveryIntegrationEvent { DeliveryKey = key.Key });
						break;

					case SubscriptionType.Apple:
						_eventBus.Publish(new DoAppleDeliveryIntegrationEvent { DeliveryKey = key.Key });
						break;
				}
			}

			return deliveries.Count;
		}
	}
}
