namespace KitsorLab.PushServer.Kernel.SeedWork.Queue
{
	using System;
	using System.Collections.Concurrent;

	public class WorkItemsQueue<T> : IWorkItemsQueue<T>
	{
		private ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

		public int Count => _items.Count;

		/// <param name="item"></param>
		public void QueueItem(T item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			_items.Enqueue(item);
		}

		/// <param name="item"></param>
		/// <returns></returns>
		public bool DequeueItem(out T item)
		{
			return _items.TryDequeue(out item);
		}
	}
}
