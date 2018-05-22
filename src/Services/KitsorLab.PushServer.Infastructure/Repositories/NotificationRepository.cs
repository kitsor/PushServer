namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public class NotificationRepository : RepositoryBase<PushServerDbContext, Notification>, INotificationRepository
	{
		public NotificationRepository(PushServerDbContext context)
			: base(context)
		{
		}

		/// <param name="key"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<Notification> GetByKeyAsync(long key, bool isReadOnly = true)
		{
			return CreateQuery(Context.Notifications, isReadOnly)
							.FirstOrDefaultAsync(x => x.NotificationKey == key);
		}

		/// <param name="id"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<Notification> GetById(Guid id, bool isReadOnly = true)
		{
			return CreateQuery(Context.Notifications, isReadOnly)
							.FirstOrDefaultAsync(x => x.NotificationId == id);
		}

		/// <typeparam name="TKey"></typeparam>
		/// <param name="limit"></param>
		/// <param name="offset"></param>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<List<Notification>> GetListAsync<TKey>(int limit, int offset, Expression<Func<Notification, TKey>> orderBy,
			bool orderByDesc = false, bool isReadOnly = true)
		{
			var query = GetListQuery(orderBy, orderByDesc, isReadOnly);
			return PaginateQuery(query, limit, offset).ToListAsync();
		}

		/// <typeparam name="TKey"></typeparam>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<long> GetListTotalAsync<TKey>(Expression<Func<Notification, TKey>> orderBy,
			bool orderByDesc = false, bool isReadOnly = true)
		{
			var query = GetListQuery(orderBy, orderByDesc, isReadOnly);
			return query.LongCountAsync();
		}

		/// <typeparam name="TKey"></typeparam>
		/// <param name="orderBy"></param>
		/// <param name="orderByDesc"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		private IQueryable<Notification> GetListQuery<TKey>(Expression<Func<Notification, TKey>> orderBy,
			bool orderByDesc = false, bool isReadOnly = true)
		{
			IList<Expression<Func<Notification, bool>>> filters = new List<Expression<Func<Notification, bool>>>();

			return CreateListQuery(filters, orderBy, orderByDesc, isReadOnly);
		}
	}
}
