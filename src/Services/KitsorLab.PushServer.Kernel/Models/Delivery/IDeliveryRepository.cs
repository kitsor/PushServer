namespace KitsorLab.PushServer.Kernel.Models.Delivery
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IDeliveryRepository : IRepository<Delivery>
	{
		Task<List<Delivery>> GetByKeysAsync(IEnumerable<long> key, bool isReadOnly = true);
	}
}
