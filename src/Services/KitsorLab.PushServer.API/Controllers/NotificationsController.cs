namespace KitsorLab.PushServer.API.Controllers
{
	using System;
	using System.Threading.Tasks;
	using KitsorLab.PushServer.API.Application.Commands;
	using KitsorLab.PushServer.API.Attributes.Filters;
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using MediatR;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	[Produces("application/json")]
	[Route("/notifications")]
	//[Authorize]
	[ValidateModel]
	public class NotificationsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly INotificationRepository _repository;

		/// <param name="mediator"></param>
		public NotificationsController(IMediator mediator, INotificationRepository repository)
			: base()
		{
			_mediator = mediator;
			_repository = repository;
		}

		[HttpGet]
		public async Task<IActionResult> Get(NotificationListRequest model)
		{
			var command = new GetNotificationListCommand(model);
			var response = await _mediator.Send(command);

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] NotificationRequest model)
		{
			AddNotificationCommand command = new AddNotificationCommand(model);
			Notification snotification = await _mediator.Send(command);

			return StatusCode(StatusCodes.Status201Created, new ApiResponse());
		}

		[AllowAnonymous]
		[HttpGet("go/{notificationId:Guid}")]
		public async Task<IActionResult> Go(Guid notificationId)
		{
			Notification notification = await _repository.GetById(notificationId);
			if (notification == null)
			{
				return Redirect("https://google.com");
			}

			return Redirect(notification.Url);
		}
	}
}