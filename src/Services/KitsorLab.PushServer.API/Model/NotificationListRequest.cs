namespace KitsorLab.PushServer.API.Model
{
	public class NotificationListRequest : PaginatedRequest
	{
		public bool OrderByDesc { get; set; } = false;
	}
}
