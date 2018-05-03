namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using Newtonsoft.Json;

	public class PushNotificationPayload
	{
		[JsonProperty("notification")]
		public PushNotification Notification { get; private set; }

		public PushNotificationPayload(PushNotification notification)
		{
			Notification = notification;
		}
	}
}
