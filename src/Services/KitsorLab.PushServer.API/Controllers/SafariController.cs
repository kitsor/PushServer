namespace KitsorLab.PushServer.API.Controllers
{
	using System.Threading.Tasks;
	using KitsorLab.PushServer.PNS.ApplePush.Safari;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	[Produces("application/json")]
	[Route("/safari")]
	public class SafariController : Controller
	{
		private ILogger<SafariController> _logger;
		private PushPackage _pushPackage;

		public SafariController(ILogger<SafariController> logger, PushPackage pushPackage) : base()
		{
			_logger = logger;
			_pushPackage = pushPackage;
		}

		[HttpPost("v{version:int}/devices/{deviceToken}/registrations/{websitePushID}")]
		[HttpDelete("v{version:int}/devices/{deviceToken}/registrations/{websitePushID}")]
		[HttpGet]
		public IActionResult SafariSubscription()
		{
			return Ok();
		}

		[HttpGet("v{version:int}/pushPackages")]
		[HttpPost("v{version:int}/pushPackages/{websitePushId}")]
		public async Task<IActionResult> GetSafariPackage(string websitePushId)
		{
			_pushPackage.GenerateManifestJson();
			_pushPackage.CreateSign();
			return File(await _pushPackage.CompressPackage(), "application/zip", "safari.pushpackage.zip");
		}

		[HttpPost("v{version:int}/log")]
		public IActionResult SafariErrors([FromBody] SafariErrorLog model)
		{
			foreach (string log in model.Logs)
			{
				_logger.LogError($"Safari errors: {log}");
			}
			return Ok();
		}

		public class SafariErrorLog
		{
			public string[] Logs;
		}
	}
}