namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using MediatR;
	using System.Threading;
	using System.Threading.Tasks;

	public class AddDeliveryCommandHandler : IRequestHandler<AddDeliveryCommand, Delivery>
	{
		private readonly IDeliveryRepository _deliveryRepository;

		public AddDeliveryCommandHandler(IDeliveryRepository deliveryRepository)
		{
			_deliveryRepository = deliveryRepository;
		}

		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<Delivery> Handle(AddDeliveryCommand command, CancellationToken cancellationToken)
		{
			DeliveryRequest request = command.Delivery;
			Delivery delivery = new Delivery(request.NotificationKey, request.SubscriptionKey, request.ScheduledOn);

			delivery = await _deliveryRepository.AddAsync(delivery);
			await _deliveryRepository.UnitOfWork.SaveEntitiesAsync();
			return delivery;
		}
	}
}
