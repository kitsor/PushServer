namespace KitsorLab.PushServer.API.Model
{
	using KitsorLab.PushServer.Kernel.Models.Subscription;

	public class SubscriptionListRequest : PaginatedRequest
	{
		public SubscriptionType? Type { get; set; }
		public bool OrderByDesc { get; set; } = false;
	}
}
