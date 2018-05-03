namespace KitsorLab.PushServer.Infastructure.Repositories
{
	using KitsorLab.PushServer.Kernel.Models.Delivery;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class DeliveryRepository : RepositoryBase<PushServerDbContext, Delivery>, IDeliveryRepository
	{
		public DeliveryRepository(PushServerDbContext context)
			: base(context)
		{
		}

		/// <param name="key"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public Task<Delivery> GetByKeyAsync(long key, bool isReadOnly = true)
		{
			return CreateQuery(Context.Deliveries, isReadOnly)
							.FirstOrDefaultAsync(x => x.DeliveryKey == key);
		}

		/// <param name="keys"></param>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public virtual Task<List<Delivery>> GetByKeysAsync(IEnumerable<long> keys, bool isReadOnly = true)
		{
			return CreateQuery(Context.Deliveries, isReadOnly)
							.Where(x => keys.Contains(x.DeliveryKey))
							.ToListAsync();
		}
	}
}
