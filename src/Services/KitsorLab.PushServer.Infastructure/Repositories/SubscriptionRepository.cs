namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.Models.Subscription;
	using Microsoft.EntityFrameworkCore;
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
	}
}
