namespace KitsorLab.PushServer.API.Controllers
{
	using System.Net;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.API.Application.Commands;
	using KitsorLab.PushServer.API.Attributes.Filters;
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	[Produces("application/json")]
	[Route("/subscriptions")]
	[Authorize]
	[ValidateModel]
	public class SubscriptionsController : Controller
	{
		private readonly IMediator _mediator;

		/// <param name="mediator"></param>
		public SubscriptionsController(IMediator mediator)
			: base()
		{
			_mediator = mediator;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Create([FromBody] SubscriptionRequest model)
		{
			IPAddress ipAddress = HttpContext.Connection.RemoteIpAddress;
			AddSubscriptionCommand command = new AddSubscriptionCommand(model, ipAddress);
			Subscription subscription = await _mediator.Send(command);

			return StatusCode(StatusCodes.Status201Created, new ApiResponse<dynamic>(new { subscription.UserId }));
		}

		[HttpDelete("uid/{userId}")]
		[AllowAnonymous]
		public async Task<IActionResult> Delete([FromRoute] string userId)
		{
			DeleteSubscriptionCommand command = new DeleteSubscriptionCommand(userId);
			await _mediator.Send(command);

			return StatusCode(StatusCodes.Status200OK);
		}
	}
}