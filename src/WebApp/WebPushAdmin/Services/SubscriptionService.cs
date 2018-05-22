namespace KitsorLab.WebApp.WebPushAdmin.Services
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient;
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using Microsoft.Extensions.Options;
	using System.Threading.Tasks;

	public class SubscriptionService : ISubscriptionService
	{
		public PushServerClient _client;

		public SubscriptionService(IOptions<PushServerClientSettings> clientSettings)
		{
			_client = new PushServerClient(clientSettings);
		}

		/// <param name="type"></param>
		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public async Task<GetEntriesResponse<Subscription>> GetSubscriptionsAsync(SubscriptionType? type, int page, int limit)
		{
			return await _client.GetSubscriptions(type, page, limit);
		}
	}
}
