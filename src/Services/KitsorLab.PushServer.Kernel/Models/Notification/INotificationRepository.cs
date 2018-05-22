namespace KitsorLab.PushServer.Kernel.Models.Notification
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface INotificationRepository : IRepository<Notification>
	{
		/// <param name="id"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<Notification> GetById(Guid id, bool isReadOnly = true);

		/// <typeparam name="TKey"></typeparam>
		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<List<Notification>> GetListAsync<TKey>(int limit, int offset, Expression<Func<Notification, TKey>> orderBy,
			bool orderByDesc = false, bool isReadOnly = true);

		/// <typeparam name="TKey"></typeparam>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<long> GetListTotalAsync<TKey>(Expression<Func<Notification, TKey>> orderBy,
			bool orderByDesc = false, bool isReadOnly = true);
	}
}
