namespace KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models
{
	public class Subscription
	{
		public long SubscriptionKey { get; set; }
		public string UserId { get; set; }
		public string Endpoint { get; set; }
		public string PublicKey { get; set; }
		public string Token { get; set; }
		public string DeviceToken { get; set; }
		public SubscriptionType Type { get; set; }
		public string IPAddress { get; set; }
	}
}
