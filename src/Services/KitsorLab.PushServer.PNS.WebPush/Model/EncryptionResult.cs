namespace KitsorLab.PushServer.PNS.WebPush.Model
{
	using KitsorLab.PushServer.PNS.WebPush.Utils;

	class EncryptionResult
	{
		public byte[] PublicKey { get; set; }
		public byte[] Payload { get; set; }
		public byte[] Salt { get; set; }

		public string Base64EncodePublicKey()
		{
			return UrlBase64.Encode(PublicKey);
		}

		public string Base64EncodeSalt()
		{
			return UrlBase64.Encode(Salt);
		}
	}
}
