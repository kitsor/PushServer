namespace KitsorLab.PushServer.PNS.ApplePush
{
	using Newtonsoft.Json;
	using System;

	public class PushNotification
	{
		[JsonProperty("title")]
		public string Title { get; }

		[JsonProperty("body")]
		public string Message { get; }

		[JsonProperty("action")]
		public string Action { get; }

		public PushNotification(string title, string message, string action)
		{
			Title = title ?? throw new ArgumentNullException(title);
			Message = message ?? throw new ArgumentNullException(message);
			Action = action ?? throw new ArgumentNullException(action);
		}
	}
}
