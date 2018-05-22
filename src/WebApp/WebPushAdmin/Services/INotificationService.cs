using KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models;
using System.Threading.Tasks;

namespace KitsorLab.WebApp.WebPushAdmin.Services
{
	public interface INotificationService
	{
		/// <param name="page"></param>
		/// <param name="limit"></param>
		/// <returns></returns>
		Task<GetEntriesResponse<Notification>> GetNotificationsAsync(int page, int limit);
	}
}
