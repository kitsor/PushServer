namespace KitsorLab.PushServer.PNS.ApplePush.Utils
{
	using KitsorLab.PushServer.PNS.ApplePush.Models;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	internal sealed class JWTUtils
	{
		private JWTUtils()
		{
		}

		/// <param name="privateKeyPath"></param>
		/// <param name="keyId"></param>
		/// <param name="teamId"></param>
		/// <returns></returns>
		public static AppleJWT CreateAPNJWT(string privateKeyPath, string keyId, string teamId)
		{
			string privateKeyContent = File.ReadAllText(privateKeyPath);
			IList<string> privateKey = privateKeyContent.Split('\n').ToList().Where(x => !x.Contains("-----")).ToList();
			byte[] pkey = Convert.FromBase64String(string.Join("", privateKey));

			return CreateAPNJWT(pkey, keyId, teamId);
		}

		/// <param name="privateKey"></param>
		/// <param name="keyId"></param>
		/// <param name="teamId"></param>
		/// <returns></returns>
		public static AppleJWT CreateAPNJWT(byte[] privateKey, string keyId, string teamId)
		{
			dynamic header = new { alg = "ES256", kid = keyId };
			dynamic payload = new { iss = teamId, iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

			return CreateJWT(privateKey, JsonConvert.SerializeObject(header), JsonConvert.SerializeObject(payload));
		}

		/// <param name="privateKey"></param>
		/// <param name="header"></param>
		/// <param name="payload"></param>
		/// <returns></returns>
		private static AppleJWT CreateJWT(byte[] privateKey, string header, string payload)
		{
			CngKey key = CngKey.Import(privateKey, CngKeyBlobFormat.Pkcs8PrivateBlob);
			string pubKey = Base64UrlEncode(key.Export(CngKeyBlobFormat.EccPublicBlob));
			string pubKeyB64 = Convert.ToBase64String(key.Export(CngKeyBlobFormat.GenericPublicBlob));

			using (ECDsaCng dsa = new ECDsaCng(key))
			{
				dsa.HashAlgorithm = CngAlgorithm.Sha256;
				var unsignedJwtData =
						Base64UrlEncode(Encoding.UTF8.GetBytes(header)) + "." + Base64UrlEncode(Encoding.UTF8.GetBytes(payload));
				var signature =
						dsa.SignData(Encoding.UTF8.GetBytes(unsignedJwtData));
				return new AppleJWT { Content = unsignedJwtData + "." + Base64UrlEncode(signature), IssuedAt = DateTime.UtcNow };
			}
		}

		/// <param name="data"></param>
		/// <returns></returns>
		private static string Base64UrlEncode(byte[] data)
		{
			return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').TrimEnd('=');
		}
	}
}
