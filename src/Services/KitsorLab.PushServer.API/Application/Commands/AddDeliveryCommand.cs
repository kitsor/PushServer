namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using MediatR;

	public class AddDeliveryCommand : IRequest<Delivery>
	{
		public DeliveryRequest Delivery { get; private set; }

		public AddDeliveryCommand(DeliveryRequest delivery)
		{
			Delivery = delivery;
		}
	}
}
