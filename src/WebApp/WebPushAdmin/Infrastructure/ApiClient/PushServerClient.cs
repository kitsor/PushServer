namespace KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class PushServerClient
	{
		private readonly PushServerClientSettings _settings;
		private HttpClient _client;

		public PushServerClient(IOptions<PushServerClientSettings> settings)
		{
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_client = new HttpClient();
		}

		/// <param name="type"></param>
		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public async Task<GetEntriesResponse<Subscription>> GetSubscriptions(SubscriptionType? type, int page, int limit)
		{
			IDictionary<string, string> urlparams = new Dictionary<string, string>
			{
				{ "page", page.ToString() },
				{ "entriesPerPage", limit.ToString() }
			};

			if (type != null)
				urlparams.Add("type", type.Value.ToString());

			string uri = _settings.BaseServiceUrl + "/subscriptions?" +
				string.Join("&", urlparams.Select(x => $"{x.Key}={x.Value}"));

			return await RunRequest<GetEntriesResponse<Subscription>>(uri);
		}

		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public async Task<GetEntriesResponse<Notification>> GetNotifications(int page, int limit)
		{
			IDictionary<string, string> urlparams = new Dictionary<string, string>
			{
				{ "page", page.ToString() },
				{ "entriesPerPage", limit.ToString() }
			};

			string uri = _settings.BaseServiceUrl + "/notifications?" +
				string.Join("&", urlparams.Select(x => $"{x.Key}={x.Value}"));

			return await RunRequest<GetEntriesResponse<Notification>>(uri);
		}

		/// <typeparam name="TResponse"></typeparam>
		/// <param name="uri"></param>
		/// <returns></returns>
		protected async Task<TResponse> RunRequest<TResponse>(string uri)
		{
			TResponse retVal = default(TResponse);

			using (HttpRequestMessage request = CreateRequest(HttpMethod.Get, uri))
			{
				HttpResponseMessage response = await _client.GetAsync(uri);
				//HttpResponseMessage response = await _client.SendAsync(request);
				string content = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					throw new Exception($"Error during executing request. HttpCode: {response.StatusCode}. Content: {content}");
				}

				retVal = JsonConvert.DeserializeObject<TResponse>(content);
			}

			return retVal;
		}

		/// <param name="method"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		protected HttpRequestMessage CreateRequest(HttpMethod method, string uri)
		{
			//TODO: add authorization
			return new HttpRequestMessage(method, uri);
		}
	}
}
