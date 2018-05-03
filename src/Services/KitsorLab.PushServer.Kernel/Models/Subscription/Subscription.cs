namespace KitsorLab.PushServer.Kernel.Models.Subscription
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;
	using System.Linq;
	using System.Net;

	public class Subscription : Entity, IAggregateRoot
	{
		public long SubscriptionKey { get; set; }
		public override long PrimaryKey => SubscriptionKey;

		public string UserId { get; private set; }
		public string Endpoint { get; private set; }
		public string PublicKey { get; private set; }
		public string Token { get; private set; }
		public string DeviceToken { get; private set; }

		public SubscriptionType Type { get; private set; }

		public long? IP { get; private set; }
		public string IPAddress
		{
			get
			{
				if (!IP.HasValue)
				{
					return null;
				}

				byte[] bytes = BitConverter.GetBytes(IP.Value)
					.Take(4).Reverse().ToArray();
				return new IPAddress(bytes).ToString();
			}
		}

		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }

		public Subscription()
		{
		}

		/// <param name="userId"></param>
		/// <param name="endpoint"></param>
		/// <param name="publickKey"></param>
		/// <param name="token"></param>
		public Subscription(string userId, string endpoint, string publickKey, string token)
		{
			Type = SubscriptionType.W3C;

			UserId = userId;
			Endpoint = endpoint;
			PublicKey = publickKey;
			Token = token;
			CreatedOn = DateTime.UtcNow;
			UpdatedOn = DateTime.UtcNow;
		}

		public Subscription(string userId, string deviceToken)
		{
			Type = SubscriptionType.Apple;

			UserId = userId;
			DeviceToken = deviceToken;
			CreatedOn = DateTime.UtcNow;
			UpdatedOn = DateTime.UtcNow;
		}

		/// <param name="ipAddress"></param>
		public void SetIpAddress(string ipAddress)
		{
			if (ipAddress == null || !System.Net.IPAddress.TryParse(ipAddress, out IPAddress ipAddr))
				throw new ArgumentException("Invalid IP address", nameof(ipAddr));

			SetIpAddress(ipAddr);
		}

		/// <param name="ipAddress"></param>
		public void SetIpAddress(IPAddress ipAddress)
		{
			if (ipAddress == null)
				throw new ArgumentNullException("Invalid IP address", nameof(ipAddress));

			byte[] bytes = ipAddress.GetAddressBytes()
				.Reverse().ToArray();
			IP = BitConverter.ToUInt32(bytes, 0);
		}

		public void SetUpdatedOnNow()
		{
			UpdatedOn = DateTime.UtcNow;
		}
	}
}
