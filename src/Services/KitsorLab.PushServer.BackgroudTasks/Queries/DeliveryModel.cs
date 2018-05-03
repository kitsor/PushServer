using KitsorLab.PushServer.Kernel.Models.Delivery;
using System;

namespace KitsorLab.PushServer.BackgroudTasks.Queries
{
	public class DeliveryModel
	{
		public long DeliveryKey { get; set; }
		public Notification Notification { get; set; }
		public Subscription Subscription { get; set; }
		public DeliveryStatus Status { get; set; }

		public DeliveryModel(dynamic data)
		{
			DeliveryKey = data.DeliveryKey;
			Notification = new Notification(data);
			Subscription = new Subscription(data);

			Status = (DeliveryStatus)data.Status;
		}
	}

	public class Notification
	{
		public long NotificationKey { get; private set; }
		public Guid NotificationId { get; private set; }
		public string Title { get; private set; }
		public string Message { get; private set; }
		public string Url { get; private set; }
		public string IconUrl { get; private set; }
		public string ImageUrl { get; private set; }

		public Notification(dynamic data)
		{
			NotificationKey = data.NotificationKey;
			NotificationId = data.NotificationId;
			Title = data.Title;
			Message = data.Message;
			Url = data.Url;
			IconUrl = data.IconUrl;
			ImageUrl = data.ImageUrl;
		}
	}

	public class Subscription
	{
		public long SubscriptionKey { get; private set; }
		public string Endpoint { get; private set; }
		public string PublicKey { get; private set; }
		public string Token { get; private set; }
		public string DeviceToken { get; private set; }

		public Subscription(dynamic data)
		{
			SubscriptionKey = data.SubscriptionKey;
			Endpoint = data.Endpoint;
			PublicKey = data.PublicKey;
			Token = data.Token;
			DeviceToken = data.DeviceToken;
		}
	}
}
