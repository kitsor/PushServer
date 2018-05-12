namespace KitsorLab.PushServer.API.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class DeliveryRequest
	{
		[Range(1, Int64.MaxValue)]
		public long NotificationKey { get; set; }

		[Range(1, Int64.MaxValue)]
		public long SubscriptionKey { get; set; }

		public DateTime? ScheduledOn { get; set; }
	}
}
