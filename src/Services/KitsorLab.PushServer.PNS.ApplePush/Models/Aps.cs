namespace KitsorLab.PushServer.PNS.ApplePush.Models
{
	using Newtonsoft.Json;
	using System.Collections.Generic;

	public class Aps
	{
		[JsonProperty("alert")]
		public PushNotification notification { get; set; }
		[JsonProperty("url-args")]
		public IList<string> UrlArgs { get; set; }

		public Aps(string title, string message, string action, IList<string> urlArgs = null)
		{
			UrlArgs = urlArgs ?? new List<string>();
			notification = new PushNotification(title, message, action);
		}
	}
}
