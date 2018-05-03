namespace KitsorLab.PushServer.Kernel.Extensions
{
	using System;
	using System.Globalization;
	using System.Security.Cryptography;
	using System.Text;

	public static class HashExtensions
	{
		/// <param name="data"></param>
		/// <param name="urlBase64"></param>
		/// <returns></returns>
		public static string GetMD5HashString(this string data, bool urlBase64 = false)
		{
			byte[] hash = Encoding.UTF8.GetBytes(data).GetMD5Hash();
			return urlBase64 ? hash.ToUrlBase64() : hash.ToHex();
		}

		/// <param name="data"></param>
		/// <param name="urlBase64"></param>
		/// <returns></returns>
		public static string GetSha1HashString(this string data, bool urlBase64 = false)
		{
			byte[] hash = Encoding.UTF8.GetBytes(data).GetSha1Hash();
			return urlBase64 ? hash.ToUrlBase64() : hash.ToHex();
		}

		/// <param name="data"></param>
		/// <param name="urlBase64"></param>
		/// <returns></returns>
		public static string GetSha256HashString(this string data, bool urlBase64 = false)
		{
			byte[] hash = Encoding.UTF8.GetBytes(data).GetSha256Hash();
			return urlBase64 ? hash.ToUrlBase64() : hash.ToHex();
		}

		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] GetSha1Hash(this byte[] data)
		{
			using (SHA1 sha1 = SHA1.Create())
			{
				byte[] hash = sha1.ComputeHash(data);
				return hash;
			}
		}

		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] GetSha256Hash(this byte[] data)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] hash = sha256.ComputeHash(data);
				return hash;
			}
		}

		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] GetMD5Hash(this byte[] data)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] hash = md5.ComputeHash(data);
				return hash;
			}
		}

		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToHex(this byte[] bytes)
		{
			StringBuilder data = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				data.AppendFormat("{0:x2}", b);
			}
			return data.ToString();
		}

		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] HexToByte(this string data)
		{
			byte[] bytes = new byte[data.Length / 2];
			try
			{
				for (int i = 0; i < data.Length / 2; i++)
					bytes[i] = byte.Parse(data.Substring(i * 2, 2), NumberStyles.HexNumber);
			}
			catch
			{
				return new byte[0];
			}
			return bytes;
		}

		/// <param name="data"></param>
		/// <returns></returns>
		public static string ToUrlBase64(this byte[] data)
		{
			return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').TrimEnd('=');
		}
	}
}
