namespace KitsorLab.PushServer.PNS.ApplePush.Connections
{
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using KitsorLab.PushServer.PNS.ApplePush.Utils;
	using Microsoft.Extensions.Logging;
	using System;
	using System.Net.Http;

	public class JwtPKeyAPNConnection : APNConnectionBase
	{
		private readonly string _privateKey;
		private readonly string _keyId;
		private readonly string _teamId;

		private AppleJWT _jwt;
		private TimeSpan _defaultJwtTtl = TimeSpan.FromMinutes(45);

		private AppleJWT JWT
		{
			get
			{
				if (_jwt == null || _jwt.IssuedAt.Add(_defaultJwtTtl) > DateTime.UtcNow)
				{
					_jwt = JWTUtils.CreateAPNJWT(_privateKey, _keyId, _teamId);
				}

				return _jwt;
			}
		}

		/// <param name="clientHandler"></param>
		/// <param name="url"></param>
		public JwtPKeyAPNConnection(HttpClientHandler clientHandler, string url, string privateKey, string keyId,
			string teamId, ILogger<JwtPKeyAPNConnection> logger)
			: base(clientHandler, url, logger)
		{
			_privateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
			_keyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
			_teamId = teamId ?? throw new ArgumentNullException(nameof(teamId));
		}

		/// <param name="url"></param>
		/// <param name="messageUuid"></param>
		/// <param name="topic"></param>
		/// <param name="expiration"></param>
		/// <param name="lowPrioriry"></param>
		/// <returns></returns>
		protected override HttpRequestMessage CreateRequestMessage(string url, string messageUuid, string topic, 
			DateTimeOffset expiration, bool lowPrioriry)
		{
			HttpRequestMessage request = base.CreateRequestMessage(url, messageUuid, topic, expiration, lowPrioriry);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", JWT.Content);

			return request;
		}

	}
}
