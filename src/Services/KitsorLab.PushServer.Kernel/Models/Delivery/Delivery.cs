namespace KitsorLab.PushServer.Kernel.Models.Delivery
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;

	public class Delivery : Entity, IAggregateRoot
	{
		public long DeliveryKey { get; private set; }
		public override long PrimaryKey => DeliveryKey;

		private long _notificationKey;
		public long NotificationKey => _notificationKey;

		private long _subscriptionKey;
		public long SubscriptionKey => _subscriptionKey;

		public DeliveryStatus Status { get; private set; }
		public DateTime? ScheduledOn { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public DateTime UpdatedOn { get; private set; }

		public Delivery()
		{
		}

		/// <param name="notificationKey"></param>
		/// <param name="subscriptionKey"></param>
		public Delivery(long notificationKey, long subscriptionKey, DateTime? scheduledOn)
		{
			_notificationKey = notificationKey;
			_subscriptionKey = subscriptionKey;
			ScheduledOn = scheduledOn;
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
