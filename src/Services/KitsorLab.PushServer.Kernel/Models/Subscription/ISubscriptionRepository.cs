namespace KitsorLab.PushServer.Kernel.Models.Subscription
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface ISubscriptionRepository : IRepository<Subscription>
	{
		/// <param name="userId"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<Subscription> GetByUserIdAsync(string userId, bool isReadOnly = true);

		/// <typeparam name="TKey"></typeparam>
		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<List<Subscription>> GetListAsync<TKey>(int limit, int offset, Expression<Func<Subscription, TKey>> orderBy,
			bool orderByDesc = false, SubscriptionType? type = null, bool isReadOnly = true);

		/// <typeparam name="TKey"></typeparam>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="type"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		Task<long> GetListTotalAsync<TKey>(Expression<Func<Subscription, TKey>> orderBy, bool orderByDesc = false, 
			SubscriptionType? type = null, bool isReadOnly = true);
	}
}
