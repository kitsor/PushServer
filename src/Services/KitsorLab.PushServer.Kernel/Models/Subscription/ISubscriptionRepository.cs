namespace KitsorLab.PushServer.Kernel.Models.Subscription
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System.Threading.Tasks;

	public interface ISubscriptionRepository : IRepository<Subscription>
	{
		Task<Subscription> GetByUserIdAsync(string userId, bool isReadOnly = true);
	}
}
