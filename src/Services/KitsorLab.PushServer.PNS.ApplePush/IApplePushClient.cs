using KitsorLab.PushServer.PNS.ApplePush.Connections;

namespace KitsorLab.PushServer.PNS.ApplePush
{
	public interface IApplePushClient
	{
		/// <returns></returns>
		IApplePNConnection GetConnection();
	}
}
