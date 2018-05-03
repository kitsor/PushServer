namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System.Threading.Tasks;

	public interface IWebPushService
	{
		/// <param name="endpoint"></param>
		/// <param name="publicKey"></param>
		/// <param name="token"></param>
		/// <param name="payload"></param>
		/// <returns></returns>
		Task<ProcessResult> SendAsync(string endpoint, string publicKey, string token, PushNotificationPayload payload);
	}
}