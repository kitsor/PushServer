namespace KitsorLab.PushServer.API.Model
{
	public class PaginatedResponse<T> : ApiResponse<T>
	{
		public PaginationResult Pagination { get; set; }

		public PaginatedResponse(T data, long total, int page, int totalPages, int maxPerPage, string errorMsg = null, int errorCode = 0)
			: base(data, errorMsg, errorCode)
		{
			Pagination = new PaginationResult()
			{
				Total = total,
				Page = page,
				TotalPages = totalPages,
				MaxPerPage = maxPerPage,
			};
		}

		public class PaginationResult
		{
			public long Total { get; set; }
			public int Page { get; set; }
			public int TotalPages { get; set; }
			public int MaxPerPage { get; set; }
		}
	}
}
