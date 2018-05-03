namespace KitsorLab.PushServer.BackgroudTasks.Tasks.Queue
{
	using KitsorLab.PushServer.Kernel.SeedWork.Queue;

	public interface IDeliveryItemsQueue : IWorkItemsQueue<long>
	{
	}
}
