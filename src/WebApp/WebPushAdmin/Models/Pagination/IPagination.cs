namespace KitsorLab.WebApp.WebPushAdmin.Models.Pagination
{
	public interface IPagination
	{
		int PageIndex { get; }
		int TotalPages { get; }
		long TotalItems { get; }

		bool HasPreviousPage { get; }
		int? PreviousPage { get; }

		bool HasNextPage { get; }
		int? NextPage { get; }

		int PageSize { get; }

		int Count { get; }
	}
}
