namespace KitsorLab.PushServer.API.Application.Commands
{
	using KitsorLab.PushServer.API.Model;
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using MediatR;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	public class GetSubscriptionListCommandHandler : IRequestHandler<GetSubscriptionListCommand, PaginatedResponse<IList<Subscription>>>
	{
		private readonly ISubscriptionRepository _subscriptionRepository;

		/// <param name="subscriptionRepository"></param>
		public GetSubscriptionListCommandHandler(ISubscriptionRepository subscriptionRepository)
		{
			_subscriptionRepository = subscriptionRepository
				?? throw new ArgumentException(nameof(subscriptionRepository));
		}

		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<PaginatedResponse<IList<Subscription>>> Handle(GetSubscriptionListCommand command, CancellationToken cancellationToken)
		{
			SubscriptionListRequest request = command.Request;
			IList<Subscription> list = 
				await _subscriptionRepository.GetListAsync(request.GetLimit(), request.GetOffset(), x => x.CreatedOn, true, request.Type);
			long total = await _subscriptionRepository.GetListTotalAsync(x => x.CreatedOn, true, request.Type);

			int totalPages = (int)Math.Ceiling((decimal)total / request.EntriesPerPage);
			var result = new PaginatedResponse<IList<Subscription>>(list, total, request.Page, totalPages, request.GetLimit());
			return result;
		}
	}
}
