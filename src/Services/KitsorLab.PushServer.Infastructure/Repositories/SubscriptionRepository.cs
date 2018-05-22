namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public class SubscriptionRepository : RepositoryBase<PushServerDbContext, Subscription>, ISubscriptionRepository
	{
		public SubscriptionRepository(PushServerDbContext context)
			: base(context)
		{
		}

		/// <param name="userId"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<Subscription> GetByUserIdAsync(string userId, bool isReadOnly = true)
		{
			return CreateQuery(_context.Subscriptions, isReadOnly)
							.FirstOrDefaultAsync(x => x.UserId == userId);
		}

		/// <param name="key"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<Subscription> GetByKeyAsync(long key, bool isReadOnly = true)
		{
			return CreateQuery(Context.Subscriptions, isReadOnly)
							.FirstOrDefaultAsync(x => x.SubscriptionKey == key);
		}

		/// <typeparam name="TKey"></typeparam>
		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<List<Subscription>> GetListAsync<TKey>(int limit, int offset, Expression<Func<Subscription, TKey>> orderBy, 
			bool orderByDesc = false, SubscriptionType? type = null, bool isReadOnly = true)
		{
			var query = GetListQuery(orderBy, orderByDesc, type, isReadOnly);
			return PaginateQuery(query, limit, offset).ToListAsync();
		}

		/// <typeparam name="TKey"></typeparam>
		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="type"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<long> GetListTotalAsync<TKey>(Expression<Func<Subscription, TKey>> orderBy,
			bool orderByDesc = false, SubscriptionType? type = null, bool isReadOnly = true)
		{
			var query = GetListQuery(orderBy, orderByDesc, type, isReadOnly);
			return query.LongCountAsync();			
		}

		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="type"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		private IQueryable<Subscription> GetListQuery<TKey>(Expression<Func<Subscription, TKey>> orderBy,
			bool orderByDesc = false, SubscriptionType? type = null, bool isReadOnly = true)
		{
			IList<Expression<Func<Subscription, bool>>> filters = new List<Expression<Func<Subscription, bool>>>();
			if (type != null)
			{
				filters.Add((x => x.Type == type));
			}

			return CreateListQuery(filters, orderBy, orderByDesc, isReadOnly);
		}
	}
}
