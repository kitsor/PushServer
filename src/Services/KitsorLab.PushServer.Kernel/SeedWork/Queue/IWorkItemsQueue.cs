namespace KitsorLab.PushServer.Kernel.SeedWork.Queue
{
	public interface IWorkItemsQueue<T>
	{
		/// <param name="item"></param>
		void QueueItem(T item);

		/// <param name="item"></param>
		/// <returns></returns>
		bool DequeueItem(out T item);

		int Count { get; }
	}
}
