namespace KitsorLab.PushServer.API.Application.IntegrationEvents.Handlers
{
	using KitsorLab.BuildingBlocks.EventBus.Handlers;
	using KitsorLab.PushServer.API.Application.Commands;
	using KitsorLab.PushServer.API.Application.IntegrationEvents.Events;
	using KitsorLab.PushServer.API.Model;
	using MediatR;
	using System.Threading.Tasks;

	public class AddDeliveryIntegrationEventHandler : IIntegrationEventHandler<AddDeliveryIntegrationEvent>
	{
		private readonly IMediator _mediator;

		/// <param name="mediator"></param>
		public AddDeliveryIntegrationEventHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <param name="event"></param>
		/// <returns></returns>
		public async Task Handle(AddDeliveryIntegrationEvent @event)
		{
			DeliveryRequest request = new DeliveryRequest
			{
				NotificationKey = @event.NotificationKey,
				SubscriptionKey = @event.SubscriptionKey,
				ScheduledOn = @event.ScheduledOn,
			};

			AddDeliveryCommand command = new AddDeliveryCommand(request);
			await _mediator.Send(command);
		}
	}
}
