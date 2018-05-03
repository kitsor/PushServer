namespace KitsorLab.PushServer.Kernel.Models.Notification
{
	using KitsorLab.PushServer.Kernel.SeedWork;
	using System;
	using System.Threading.Tasks;

	public interface INotificationRepository : IRepository<Notification>
	{
		Task<Notification> GetById(Guid id, bool isReadOnly = true);
	}
}
