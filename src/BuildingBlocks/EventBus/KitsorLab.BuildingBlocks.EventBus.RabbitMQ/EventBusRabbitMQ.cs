namespace KitsorLab.BuildingBlocks.EventBus.RabbitMQ
{
	using global::RabbitMQ.Client;
	using global::RabbitMQ.Client.Events;
	using global::RabbitMQ.Client.Exceptions;
	using KitsorLab.BuildingBlocks.EventBus.Events;
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.BuildingBlocks.EventBus.SubscriptionsManagers;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Polly;
	using Polly.Retry;
	using System;
	using System.Collections.Generic;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading.Tasks;

	public class EventBusRabbitMQ : IEventBus, IDisposable
	{
		private const string DEFAULT_BROKER_NAME = "KitsorLab.Broker";
		private readonly string _brokerName;
		private readonly IRabbitMQPersistentConnection _persistentConnection;
		private readonly ILogger<EventBusRabbitMQ> _logger;
		private readonly IEventBusSubscriptionsManager _subsManager;
		private readonly IServiceProvider _serviceProvider;
		private readonly int _retryCount;

		private IModel _consumerChannel;
		private string _queueName;

		public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, IEventBusSubscriptionsManager subsManager, string brokerName,
			ILogger<EventBusRabbitMQ> logger, IServiceProvider serviceProvider, string queueName, int retryCount = 5)
		{
			_persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
			_subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
			_brokerName = brokerName ?? DEFAULT_BROKER_NAME;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
			_consumerChannel = CreateConsumerChannel();
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_retryCount = retryCount >= 0 ? retryCount : throw new ArgumentOutOfRangeException(nameof(retryCount), "Must be positive.");

			_subsManager.OnSubscriptionRemoved += SubsManager_OnSubscriptionRemoved;
		}

		/// <param name="event"></param>
		public void Publish(IntegrationEvent @event)
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}

			RetryPolicy policy = Policy.Handle<BrokerUnreachableException>()
				.Or<SocketException>()
				.WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
					(ex, time) =>
					{
						_logger.LogWarning(ex.ToString());
					}
				);

			using (IModel channel = _persistentConnection.CreateModel())
			{
				string eventName = @event.GetType().Name;
				channel.ExchangeDeclare(exchange: _brokerName,	type: "direct");

				string message = JsonConvert.SerializeObject(@event);
				byte[] body = Encoding.UTF8.GetBytes(message);

				policy.Execute(() =>
				{
					IBasicProperties properties = channel.CreateBasicProperties();
					properties.DeliveryMode = 2; // persistent

					channel.BasicPublish(exchange: _brokerName, routingKey: eventName, mandatory: true, basicProperties: properties, body: body);
				});
			}
		}

		/// <typeparam name="TH"></typeparam>
		/// <param name="eventName"></param>
		public void SubscribeDynamic<TH>(string eventName)
			where TH : IDynamicIntegrationEventHandler
		{
			DoInternalSubscription(eventName);
			_subsManager.AddDynamicSubscription<TH>(eventName);
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TH"></typeparam>
		public void Subscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{
			string eventName = _subsManager.GetEventKey<T>();
			DoInternalSubscription(eventName);
			_subsManager.AddSubscription<T, TH>();
		}

		/// <typeparam name="TH"></typeparam>
		/// <param name="eventName"></param>
		public void UnsubscribeDynamic<TH>(string eventName)
			where TH : IDynamicIntegrationEventHandler
		{
			_subsManager.RemoveDynamicSubscription<TH>(eventName);
		}

		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TH"></typeparam>
		public void Unsubscribe<T, TH>()
			where TH : IIntegrationEventHandler<T>
			where T : IntegrationEvent
		{
			_subsManager.RemoveSubscription<T, TH>();
		}

		/// <param name="eventName"></param>
		private void DoInternalSubscription(string eventName)
		{
			bool containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
			if (!containsKey)
			{
				if (!_persistentConnection.IsConnected)
				{
					_persistentConnection.TryConnect();
				}

				using (IModel channel = _persistentConnection.CreateModel())
				{
					channel.QueueBind(exchange: _brokerName, queue: _queueName, routingKey: eventName);
				}
			}
		}

		/// <returns></returns>
		private IModel CreateConsumerChannel()
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}

			IModel channel = _persistentConnection.CreateModel();
			channel.ExchangeDeclare(exchange: _brokerName, type: "direct");
			channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false,	arguments: null);

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += async (model, ea) =>
			{
				string eventName = ea.RoutingKey;
				string message = Encoding.UTF8.GetString(ea.Body);

				await ProcessEvent(eventName, message);
				channel.BasicAck(ea.DeliveryTag, multiple: false);
			};

			channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
			channel.CallbackException += (sender, ea) =>
			{
				_consumerChannel.Dispose();
				_consumerChannel = CreateConsumerChannel();
			};

			return channel;
		}

		/// <param name="eventName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		private async Task ProcessEvent(string eventName, string message)
		{
			if (_subsManager.HasSubscriptionsForEvent(eventName))
			{
				using (IServiceScope scope = _serviceProvider.CreateScope())
				{
					IServiceProvider services = scope.ServiceProvider;
					IEnumerable<SubscriptionInfo> subscriptions = _subsManager.GetHandlersForEvent(eventName);
					foreach (SubscriptionInfo subscription in subscriptions)
					{
						if (subscription.IsDynamic)
						{
							var handler = services.GetRequiredService(subscription.HandlerType) as IDynamicIntegrationEventHandler;
							dynamic eventData = JObject.Parse(message);
							await handler.Handle(eventData);
						}
						else
						{
							Type eventType = _subsManager.GetEventTypeByName(eventName);
							var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
							var handler = services.GetRequiredService(subscription.HandlerType);
							Type concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
							await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
						}
					}
				}
			}
		}

		/// <param name="sender"></param>
		/// <param name="eventName"></param>
		private void SubsManager_OnSubscriptionRemoved(object sender, string eventName)
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}

			using (IModel channel = _persistentConnection.CreateModel())
			{
				channel.QueueUnbind(exchange: _brokerName, queue: _queueName, routingKey: eventName);

				if (_subsManager.IsEmpty)
				{
					_queueName = string.Empty;
					_consumerChannel.Close();
				}
			}
		}

		public void Dispose()
		{
			if (_consumerChannel != null)
			{
				_consumerChannel.Dispose();
			}
			_subsManager.Clear();
		}
	}
}
