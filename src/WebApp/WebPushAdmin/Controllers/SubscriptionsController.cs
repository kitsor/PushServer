namespace KitsorLab.WebApp.WebPushAdmin.Controllers
{
	using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
	using KitsorLab.WebApp.WebPushAdmin.Models.Pagination;
	using KitsorLab.WebApp.WebPushAdmin.Models.Subscriptions;
	using KitsorLab.WebApp.WebPushAdmin.Services;
	using Microsoft.AspNetCore.Mvc;
	using System.Threading.Tasks;

	public class SubscriptionsController : Controller
	{
		public const string ROUTE_INDEX = "subscriptions";

		private readonly ISubscriptionService _subscriptionService;

		public SubscriptionsController(ISubscriptionService subscriptionService)
			: base()
		{
			_subscriptionService = subscriptionService;
		}

		public async Task<IActionResult> Index(SubscriptionsRequest model)
		{
			var result = await _subscriptionService.GetSubscriptionsAsync(model.Type, model.Page, model.MaxPerPage);

			var subscriptions = new PaginatedList<Subscription>(result.Data, result.Pagination.Total, result.Pagination.Page, result.Pagination.MaxPerPage);
			return View(new SubscriptionsResponse { Subscriptions = subscriptions });
		}
	}
}