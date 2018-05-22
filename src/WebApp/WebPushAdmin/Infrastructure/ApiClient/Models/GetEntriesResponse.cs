namespace KitsorLab.WebApp.WebPushAdmin.Infrastructure.ApiClient.Models
{
	using System.Collections.Generic;

	public class GetEntriesResponse<T>
	{
		public IList<T> Data { get; set; }
		public PaginationResult Pagination { get; set; }
	}

	public class PaginationResult
	{
		public long Total { get; set; }
		public int Page { get; set; }
		public int TotalPages { get; set; }
		public int MaxPerPage { get; set; }
	}
}
