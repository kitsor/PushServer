namespace KitsorLab.PushServer.PNS.ApplePush.Models
{
	using Newtonsoft.Json;
	using Org.BouncyCastle.Crypto.Parameters;
	using Org.BouncyCastle.Crypto.Signers;
	using Org.BouncyCastle.OpenSsl;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	internal class AppleJWT
	{
		public DateTimeOffset IssuedAt { get; private set; }
		public string Content { get; private set; }

		private readonly string _teamId;
		private readonly string _keyId;
		private readonly string _pkeyPath;

		public AppleJWT(string pkeyPath, string keyId, string teamId)
		{
			_pkeyPath = pkeyPath ?? throw new ArgumentNullException(nameof(pkeyPath));
			_keyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
			_teamId = teamId ?? throw new ArgumentNullException(nameof(teamId));

			Renew();
		}

		/// <returns></returns>
		public void Renew()
		{
			DateTimeOffset issuedAt = DateTimeOffset.UtcNow;
			string header = JsonConvert.SerializeObject(GetHeader());
			string payload = JsonConvert.SerializeObject(GetPayload(issuedAt));

			var unsignedJwtData =
						Base64UrlEncode(Encoding.UTF8.GetBytes(header)) + "." + Base64UrlEncode(Encoding.UTF8.GetBytes(payload));

			byte[] hash = GetHash(unsignedJwtData);
			byte[] signature = GetSignature(hash);

			Content = unsignedJwtData + "." + Base64UrlEncode(signature);
			IssuedAt = issuedAt;
		}

		/// <param name="data"></param>
		/// <returns></returns>
		private byte[] GetHash(string data)
		{
			byte[] hash;
			using (SHA256 sha256 = SHA256.Create())
			{
				hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
			}
			return hash;
		}

		/// <param name="hash"></param>
		/// <returns></returns>
		private byte[] GetSignature(byte[] hash)
		{
			ECDsaSigner signer = new ECDsaSigner();
			signer.Init(true, GetPrivateKey());
			var result = signer.GenerateSignature(hash);

			// Concated to create signature
			var a = result[0].ToByteArrayUnsigned();
			var b = result[1].ToByteArrayUnsigned();

			// a,b are required to be exactly the same length of bytes
			if (a.Length != b.Length)
			{
				var largestLength = Math.Max(a.Length, b.Length);
				a = ByteArrayPadLeft(a, largestLength);
				b = ByteArrayPadLeft(b, largestLength);
			}

			return a.Concat(b).ToArray();
		}

		/// <returns></returns>
		private ECPrivateKeyParameters GetPrivateKey()
		{
			using (TextReader reader = File.OpenText(_pkeyPath))
			{
				PemReader pemReader = new PemReader(reader);
				return (ECPrivateKeyParameters)pemReader.ReadObject();
			}
		}

		/// <returns></returns>
		private IDictionary<string, string> GetHeader()
		{
			return new Dictionary<string, string>
			{
				{ "alg", "ES256" },
				{ "kid", _keyId },
			};
		}

		/// <param name="issuedAt"></param>
		/// <returns></returns>
		private IDictionary<string, string> GetPayload(DateTimeOffset issuedAt)
		{
			return new Dictionary<string, string>
			{
				{ "iss", _teamId },
				{ "iat", issuedAt.ToUnixTimeSeconds().ToString() },
			};
		}

		/// <param name="src"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		private static byte[] ByteArrayPadLeft(byte[] src, int size)
		{
			var dst = new byte[size];
			var startAt = dst.Length - src.Length;
			Array.Copy(src, 0, dst, startAt, src.Length);
			return dst;
		}

		/// <param name="data"></param>
		/// <returns></returns>
		private static string Base64UrlEncode(byte[] data)
		{
			return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').TrimEnd('=');
		}
	}
}
