namespace KitsorLab.PushServer.BackgroudTasks.Tasks.Queue
{
	using KitsorLab.PushServer.Kernel.SeedWork.Queue;

	public class DeliveryItemsQueue : WorkItemsQueue<long>, IDeliveryItemsQueue
	{
	}
}
