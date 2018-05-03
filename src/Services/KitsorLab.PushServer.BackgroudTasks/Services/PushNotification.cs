namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using Newtonsoft.Json;

	public class PushNotification
	{
		[JsonProperty("title")]
		public string Title { get; private set; }

		[JsonProperty("message")]
		public string Message { get; private set; }

		[JsonProperty("url")]
		public string Url { get; private set; }

		[JsonProperty("iconUrl")]
		public string IconUrl { get; private set; }

		[JsonProperty("imageUrl")]
		public string ImageUrl { get; private set; }

		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="iconUrl"></param>
		/// <param name="url"></param>
		/// <param name="imageUrl"></param>
		public PushNotification(string title, string message, string iconUrl, string url, string imageUrl = null)
		{
			Title = title;
			Message = message;
			IconUrl = iconUrl;
			Url = url;
			ImageUrl = imageUrl;
		}
	}
}
