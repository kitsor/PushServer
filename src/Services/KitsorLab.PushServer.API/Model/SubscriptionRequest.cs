namespace KitsorLab.PushServer.API.Model
{
	using KitsorLab.PushServer.API.Attributes;
	using System.ComponentModel.DataAnnotations;

	public class SubscriptionRequest
	{
		[RequiredIf(nameof(DeviceToken), null, ErrorMessage = "Endpoint or DeviceToken is required")]
		public string Endpoint { get; set; }

		[RequiredIf(nameof(Endpoint), null, IsInverted = true, ErrorMessage = "PublicKey is required")]
		public string PublicKey { get; set; }

		[RequiredIf(nameof(Endpoint), null, IsInverted = true, ErrorMessage = "Auth is required")]
		public string Auth { get; set; }

		[StringLength(100, MinimumLength = 10)]
		public string DeviceToken { get; set; }
	}
}
