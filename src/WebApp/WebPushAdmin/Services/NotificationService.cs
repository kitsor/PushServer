namespace KitsorLab.WebApp.WebPushAdmin.Services
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient;
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using Microsoft.Extensions.Options;
	using System.Threading.Tasks;

	public class NotificationService : INotificationService
	{
		public PushServerClient _client;

		public NotificationService(IOptions<PushServerClientSettings> clientSettings)
		{
			_client = new PushServerClient(clientSettings);
		}

		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		public async Task<GetEntriesResponse<Notification>> GetNotificationsAsync(int page, int limit)
		{
			return await _client.GetNotifications(page, limit);
		}
	}
}
