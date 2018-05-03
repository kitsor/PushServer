namespace KitsorLab.BuildingBlocks.EventBus.RabbitMQ
{
	using global::RabbitMQ.Client;
	using global::RabbitMQ.Client.Exceptions;
	using global::RabbitMQ.Client.Events;
	using Microsoft.Extensions.Logging;
	using System;
	using System.IO;
	using System.Net.Sockets;
	using Polly;
	using Polly.Retry;

	public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
	{
		private readonly IConnectionFactory _connectionFactory;
		private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
		private readonly int _retryCount;
		IConnection _connection;
		bool _disposed;

		object sync_root = new object();

		public bool IsConnected
		{
			get	{	return _connection != null && _connection.IsOpen && !_disposed;	}
		}

		/// <returns></returns>
		public IModel CreateModel()
		{
			return IsConnected 
				? _connection.CreateModel()
				: throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
		}

		/// <param name="connectionFactory"></param>
		/// <param name="logger"></param>
		/// <param name="retryCount"></param>
		public DefaultRabbitMQPersistentConnection(
			IConnectionFactory connectionFactory, 
			ILogger<DefaultRabbitMQPersistentConnection> logger, 
			int retryCount = 5)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_retryCount = retryCount;
		}

		/// <returns></returns>
		public bool TryConnect()
		{
			_logger.LogInformation("RabbitMQ Client is trying to connect");

			lock (sync_root)
			{
				if (IsConnected) return true;

				RetryPolicy policy = Policy.Handle<SocketException>()
					.Or<BrokerUnreachableException>()
					.WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
					{
						_logger.LogWarning(ex.ToString());
					}
				);

				policy.Execute(() =>
				{
					_connection = _connectionFactory.CreateConnection();
				});

				if (IsConnected)
				{
					_connection.ConnectionShutdown += OnConnectionShutdown;
					_connection.CallbackException += OnCallbackException;
					_connection.ConnectionBlocked += OnConnectionBlocked;

					_logger.LogInformation($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
					return true;
				}
				else
				{
					_logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
					return false;
				}
			}
		}

		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
		{
			if (_disposed) return;

			_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
			TryConnect();
		}

		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
		{
			if (_disposed) return;

			_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
			TryConnect();
		}

		/// <param name="sender"></param>
		/// <param name="reason"></param>
		private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
		{
			if (_disposed) return;

			_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
			TryConnect();
		}

		public void Dispose()
		{
			if (_disposed) return;
			_disposed = true;

			try
			{
				_connection.Dispose();
			}
			catch (IOException ex)
			{
				_logger.LogCritical(ex.ToString());
			}
		}
	}
}
