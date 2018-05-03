namespace KitsorLab.PushServer.PNS.ApplePush
{
	using KitsorLab.PushServer.PNS.ApplePush.Connections;
	using Microsoft.Extensions.Logging;
	using System;

	public class ApplePushClient : IApplePushClient
	{
		private readonly IApplePNConnection _connection;

		/// <param name="connection"></param>
		public ApplePushClient(IApplePNConnection connection, ILogger<ApplePushClient> logger)
		{
			_connection = connection ?? throw new ArgumentNullException(nameof(connection));
		}

		/// <returns></returns>
		public IApplePNConnection GetConnection()
		{
			return _connection;
		}
	}
}
