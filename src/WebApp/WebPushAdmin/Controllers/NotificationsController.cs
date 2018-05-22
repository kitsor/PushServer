namespace KitsorLab.WebApp.WebPushAdmin.Controllers
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using KitsorLab.WebApp.WebPushAdmin.Models.Notifications;
	using KitsorLab.WebApp.WebPushAdmin.Models.Pagination;
	using KitsorLab.WebApp.WebPushAdmin.Services;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	public class NotificationsController : Controller
	{
		public const string ROUTE_INDEX = "notifications";

		private readonly INotificationService _notificationService;

		public NotificationsController(INotificationService notificationService)
			: base()
		{
			_notificationService = notificationService;
		}

		public async Task<IActionResult> Index(NotificationsRequest model)
		{
			var result = await _notificationService.GetNotificationsAsync(model.Page, model.MaxPerPage);

			var notifications = new PaginatedList<Notification>(result.Data, result.Pagination.Total, result.Pagination.Page, result.Pagination.MaxPerPage);
			return View(new NotificationsResponse { Notifications = notifications });
		}
	}
}