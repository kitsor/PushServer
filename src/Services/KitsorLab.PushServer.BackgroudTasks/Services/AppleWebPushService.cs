namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using KitsorLab.PushServer.PNS.ApplePush;
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using KitsorLab.PushServer.PNS.ApplePush.Connections;
	using KitsorLab.PushServer.BackgroudTasks.Configuration;
	using KitsorLab.PushServer.Kernel.SeedWork;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class AppleWebPushService : IAppleWebPushService
	{
		private readonly IApplePushClient _client;

		/// <param name="options"></param>
		/// <param name="connectionLogger"></param>
		/// <param name="clientLogger"></param>
		/// <param name="serviceLogger"></param>
		public AppleWebPushService(
			IOptions<AppleWebPushOptions> options,
			ILogger<JwtPKeyAPNConnection> connectionLogger,
			ILogger<ApplePushClient> clientLogger,
			ILogger<WebPushService> serviceLogger
		)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			HttpClientHandler clientHandler = new HttpClientHandler();
			string url = options.Value.ApplePNUrl;
			string pkey = options.Value.PrivateKeyPath;
			string keyId = options.Value.PrivateKeyId;
			string teamId = options.Value.TeamId;

			var connection = new JwtPKeyAPNConnection(clientHandler, url, pkey, keyId, teamId, connectionLogger);
			_client = new ApplePushClient(connection, clientLogger);
		}

		/// <param name="deviceToken"></param>
		/// <param name="notification"></param>
		/// <returns></returns>
		public async Task<ProcessResult> SendAsync(string deviceToken, Aps aps)
		{
			ProcessResult retVal = new ProcessResult();
			try
			{
				IApplePNConnection connection = _client.GetConnection();
				await connection.SendAsync(deviceToken, aps);
			}
			catch (ApplePushException ex)
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
