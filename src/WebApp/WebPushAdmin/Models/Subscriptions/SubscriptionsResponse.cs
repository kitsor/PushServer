namespace KitsorLab.WebApp.WebPushAdmin.Models.Subscriptions
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using KitsorLab.WebApp.WebPushAdmin.Models.Pagination;

	public class SubscriptionsResponse
	{
		public PaginatedList<Subscription> Subscriptions { get; set; }
	}
}
