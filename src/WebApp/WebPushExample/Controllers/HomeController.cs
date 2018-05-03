namespace WebPushExample.Controllers
{
	using System.Diagnostics;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using WebPushExample.Models;
	
	public class HomeController : Controller
	{
		private ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger) : base()
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}
