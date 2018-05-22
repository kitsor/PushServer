namespace KitsorLab.WebApp.WebPushAdmin.Models.Notifications
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using KitsorLab.WebApp.WebPushAdmin.Models.Pagination;

	public class NotificationsResponse
	{
		public PaginatedList<Notification> Notifications { get; set; }
	}
}
