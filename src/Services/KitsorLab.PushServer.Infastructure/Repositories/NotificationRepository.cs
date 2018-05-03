namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.Models.Notification;
	using Microsoft.EntityFrameworkCore;
	using System;
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
	}
}
