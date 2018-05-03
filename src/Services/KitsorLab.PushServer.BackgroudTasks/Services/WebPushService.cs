namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using KitsorLab.PushServer.PNS.WebPush;
	using KitsorLab.PushServer.PNS.WebPush.Model;
	using KitsorLab.PushServer.BackgroudTasks.Configuration;
	using KitsorLab.PushServer.Kernel.SeedWork;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class WebPushService : IWebPushService
	{
		private readonly WebPushClient _client;
		private readonly VapidDetails _vapidDetails;

		public WebPushService(IOptions<WebPushOptions> options)
		{
			_client = new WebPushClient();
			_vapidDetails = new VapidDetails(options.Value.Subject, options.Value.PublicKey, options.Value.PrivateKey);
		}

		/// <param name="endpoint"></param>
		/// <param name="publicKey"></param>
		/// <param name="token"></param>
		/// <param name="payload"></param>
		/// <returns></returns>
		public async Task<ProcessResult> SendAsync(string endpoint, string publicKey, string token, PushNotificationPayload payload)
		{
			PushSubscription subscription = new PushSubscription(endpoint, publicKey, token);
			ProcessResult retVal = new ProcessResult();
			try
			{
				await _client.SendNotificationAsync(subscription, JsonConvert.SerializeObject(payload), _vapidDetails);
			}
			catch (WebPushException ex)
			{
				retVal.SetErrorInfo(ex.Message, (int)ex.StatusCode);
			}
			catch (HttpRequestException ex)
			{
				retVal.SetErrorInfo(ex.Message);
			}

			return retVal;
		}
	}
}
