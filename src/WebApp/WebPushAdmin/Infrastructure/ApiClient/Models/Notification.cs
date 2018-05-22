using System;

namespace KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models
{
	public class Notification
	{
		public long NotificationKey { get; set; }
		public Guid NotificationId { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public string Url { get; set; }
		public string IconUrl { get; set; }
		public string ImageUrl { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}
