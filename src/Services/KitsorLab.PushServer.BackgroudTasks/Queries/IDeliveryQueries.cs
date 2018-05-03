namespace KitsorLab.PushServer.BackgroudTasks.Queries
{
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IDeliveryQueries
	{
		Task<DeliveryModel> GetDeliveryAsync(long key);
		Task<IDictionary<long, SubscriptionType>> GetNewDeliveriesAndSetInQueue(int maxItems = 10);
	}
}
