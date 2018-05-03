namespace KitsorLab.PushServer.API.Application.Commands
{
	using MediatR;

	public class DeleteSubscriptionCommand : IRequest<bool>
	{
		public string UserId { get; private set; }

		public DeleteSubscriptionCommand(string userId)
		{
			UserId = userId;
		}
	}
}
