namespace KitsorLab.PushServer.BackgroudTasks.Services
{
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System.Threading.Tasks;

	public interface IAppleWebPushService
	{
		/// <param name="deviceToken"></param>
		/// <param name="notification"></param>
		/// <returns></returns>
		Task<ProcessResult> SendAsync(string deviceToken, Aps aps);
	}
}
