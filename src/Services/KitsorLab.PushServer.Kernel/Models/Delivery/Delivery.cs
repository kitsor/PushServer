namespace KitsorLab.PushServer.Kernel.Models.Delivery
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;

	public class Delivery : Entity, IAggregateRoot
	{
		public long DeliveryKey { get; set; }
		public override long PrimaryKey => DeliveryKey;

		private long _notificationKey;
		public long NotificationKey => _notificationKey;

		private long _subscriptionKey;
		public long SubscriptionKey => _subscriptionKey;

		public DeliveryStatus Status { get; private set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }

		public Delivery()
		{
		}

		/// <param name="notificationKey"></param>
		/// <param name="subscriptionKey"></param>
		public Delivery(long notificationKey, long subscriptionKey)
		{
			_notificationKey = notificationKey;
			_subscriptionKey = subscriptionKey;
			CreatedOn = DateTime.UtcNow;
			UpdatedOn = DateTime.UtcNow;
		}

		public void SetUnknownErrorStatus()
		{
			Status = DeliveryStatus.UnknownError;
		}

		public void SetHasBeenSentStatus()
		{
			Status = DeliveryStatus.HasBeenSent;
		}
	}
}
