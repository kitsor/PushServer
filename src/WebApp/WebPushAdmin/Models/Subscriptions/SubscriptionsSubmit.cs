namespace KitsorLab.WebApp.WebPushAdmin.Models.Subscriptions
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;

	public class SubscriptionsSubmit : PaginatedSubmit
	{
		public SubscriptionType? Type { get; set; }
	}
}
