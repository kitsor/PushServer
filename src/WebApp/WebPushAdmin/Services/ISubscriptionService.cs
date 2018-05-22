using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
using System.Threading.Tasks;

namespace KitsorLab.WebApp.WebPushAdmin.Services
{
	public interface ISubscriptionService
	{
		/// <param name="type"></param>
		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		Task<GetEntriesResponse<Subscription>> GetSubscriptionsAsync(SubscriptionType? type, int page, int limit);
	}
}
