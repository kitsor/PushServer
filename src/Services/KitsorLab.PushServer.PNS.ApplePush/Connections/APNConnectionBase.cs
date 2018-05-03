namespace KitsorLab.PushServer.PNS.ApplePush.Connections
{
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;

	public abstract class APNConnectionBase : IApplePNConnection
	{
		protected static HttpClientHandler _clientHandler;

		protected readonly string _url;
		protected readonly ILogger<APNConnectionBase> _logger;
		protected TimeSpan _defaultNotificationTTL { get; } = TimeSpan.FromDays(28);
		
		protected static HttpClient _client;
		protected HttpClient Client
		{
			get
			{
				if (_client == null)
				{
					_client = new HttpClient(_clientHandler);
				}
				return _client;
			}
		}

		/// <param name="clientHandler"></param>
		/// <param name="url"></param>
		public APNConnectionBase(HttpClientHandler clientHandler, string url, ILogger<APNConnectionBase> logger)
		{
			_clientHandler = clientHandler ?? throw new ArgumentNullException(nameof(clientHandler));
			_url = url ?? throw new ArgumentNullException(nameof(url));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <param name="deviceToken"></param>
		/// <param name="notification"></param>
		/// <param name="topic"></param>
		/// <param name="ttl"></param>
		/// <param name="messageUuid"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		public async Task SendAsync(string deviceToken, Aps aps, string topic = null, TimeSpan? ttl = null, 
			string messageUuid = null, bool lowPrioriry = false)
		{
			DateTimeOffset expiration = DateTimeOffset.UtcNow.Add(ttl ?? _defaultNotificationTTL);
			string payload = JsonConvert.SerializeObject(new { aps = aps });

			await SendAsync(deviceToken, payload, expiration, topic, messageUuid, lowPrioriry);
		}

		/// <param name="deviceToken"></param>
		/// <param name="payload"></param>
		/// <param name="expiration"></param>
		/// <param name="topic"></param>
		/// <param name="messageUuid"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		public async Task SendAsync(string deviceToken, string payload, DateTimeOffset expiration, string topic = null, 
			string messageUuid = null, bool lowPrioriry = false)
		{
			messageUuid = messageUuid ?? Guid.NewGuid().ToString();
			string url = string.Format(_url, deviceToken);

			HttpRequestMessage request = CreateRequestMessage(url, messageUuid, topic, expiration, lowPrioriry);
			request.Content = new StringContent(payload);

			HttpResponseMessage response = await Client.SendAsync(request);
			await HandleResponse(response, deviceToken);
		}

		/// <param name="url"></param>
		/// <param name="messageUuid"></param>
		/// <param name="topic"></param>
		/// <param name="expiration"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		protected virtual HttpRequestMessage CreateRequestMessage(string url, string messageUuid, string topic, DateTimeOffset expiration, 
			bool lowPrioriry)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Version = new Version(2, 0)
			};

			request.Headers.Add("apns-id", messageUuid);
			request.Headers.Add("apns-priority", lowPrioriry ? "5" : "10");
			request.Headers.Add("apns-expiration", expiration.ToUnixTimeSeconds().ToString());

			if (!string.IsNullOrEmpty(topic))
			{
				request.Headers.Add("apns-topic", topic);
			}

			return request;
		}

		/// <param name="response"></param>
		/// <param name="subscription"></param>
		protected async Task HandleResponse(HttpResponseMessage response, string deviceToken)
		{
			// Successful
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return;
			}

			// Error
			var message = $@"Received unexpected response code: {(int)response.StatusCode}";
			switch (response.StatusCode)
			{
				case HttpStatusCode.BadRequest:
					message = "Bad Request";
					break;

				case HttpStatusCode.Forbidden:
					message = "There was an error with the certificate or with the provider authentication token";
					break;

				case HttpStatusCode.MethodNotAllowed:
					message = "The request used a bad :method value. Only POST requests are supported";
					break;

				case HttpStatusCode.RequestEntityTooLarge:
					message = "The notification payload was too large";
					break;

				case (HttpStatusCode)429:
					message = "The server received too many requests for the same device token";
					break;

				case HttpStatusCode.InternalServerError:
					message = "Internal server error";
					break;

				case HttpStatusCode.ServiceUnavailable:
					message = "The server is shutting down and unavailable";
					break;

				case HttpStatusCode.Gone:
					message = "Subscription no longer valid";
					break;
			}

			string content = string.Empty;
			try
			{
				content = await response.Content.ReadAsStringAsync();
			}
			catch { }

			if (!string.IsNullOrEmpty(content))
				message += $", Body: [{content}]";

			throw new ApplePushException(message, response.StatusCode, response.Headers, deviceToken);
		}
	}
}
