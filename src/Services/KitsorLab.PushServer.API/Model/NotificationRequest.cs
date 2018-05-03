namespace KitsorLab.PushServer.API.Model
{
	using System.ComponentModel.DataAnnotations;

	public class NotificationRequest
	{
		[Required]
		[StringLength(50)]
		public string Title { get; set; }

		[Required]
		[StringLength(100)]
		public string Message { get; set; }

		[Required]
		[StringLength(150)]
		public string Url { get; set; }

		[Required]
		[StringLength(150)]
		public string IconUrl { get; set; }

		[StringLength(150)]
		public string ImageUrl { get; set; }
	}
}
