namespace KitsorLab.PushServer.PNS.ApplePush.Connections
{
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using System;
	using System.Threading.Tasks;

	public interface IApplePNConnection
	{
		/// <param name="deviceToken"></param>
		/// <param name="notification"></param>
		/// <param name="topic"></param>
		/// <param name="ttl"></param>
		/// <param name="messageUuid"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		Task SendAsync(string deviceToken, Aps aps, string topic = null, TimeSpan? ttl = null, string messageUuid = null, bool lowPrioriry = false);

		/// <param name="deviceToken"></param>
		/// <param name="payload"></param>
		/// <param name="expiration"></param>
		/// <param name="topic"></param>
		/// <param name="messageUuid"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		Task SendAsync(string deviceToken, string payload, DateTimeOffset expiration, string topic = null, string messageUuid = null, bool lowPrioriry = false);
	}
}
