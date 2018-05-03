namespace KitsorLab.PushServer.Kernel.Models.Notification
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;

	public class Notification : Entity, IAggregateRoot
	{
		public long NotificationKey { get; set; }
		public override long PrimaryKey => NotificationKey;

		public Guid NotificationId { get; set; }
		public string Title { get; private set; }
		public string Message { get; private set; }
		public string Url { get; private set; }
		public string IconUrl { get; private set; }
		public string ImageUrl { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public DateTime UpdatedOn { get; private set; }

		public Notification()
		{
		}

		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="url"></param>
		/// <param name="iconUrl"></param>
		public Notification(string title, string message, string url, string iconUrl)
		{
			NotificationId = Guid.NewGuid();

			Title = title ?? throw new ArgumentNullException(nameof(title));
			Message = message ?? throw new ArgumentNullException(nameof(message));
			Url = url ?? throw new ArgumentNullException(nameof(url));
			IconUrl = iconUrl ?? throw new ArgumentNullException(nameof(iconUrl));

			CreatedOn = DateTime.UtcNow;
			UpdatedOn = DateTime.UtcNow;
		}

		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="url"></param>
		/// <param name="iconUrl"></param>
		/// <param name="imageUrl"></param>
		public Notification(string title, string message, string url, string iconUrl, string imageUrl)
			: this(title, message, url, iconUrl)
		{
			ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
		}

	}
}
