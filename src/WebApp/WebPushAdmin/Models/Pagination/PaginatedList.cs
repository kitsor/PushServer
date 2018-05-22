namespace KitsorLab.WebApp.WebPushAdmin.Models.Pagination
{
	using System;
	using System.Collections.Generic;

	public class PaginatedList<T> : List<T>, IPagination
	{
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public long TotalItems { get; private set; }

		public bool HasPreviousPage => (PageIndex > 1);
		public int? PreviousPage => (HasPreviousPage ? PageIndex - 1 : (Nullable<int>)null);

		public bool HasNextPage => (PageIndex < TotalPages);
		public int? NextPage => (HasNextPage ? PageIndex + 1 : (Nullable<int>)null);

		public int PageSize { get; set; }

		public PaginatedList(ICollection<T> items, long total, int pageIndex, int pageSize)
			: base(items)
		{
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling((decimal)total / pageSize);
			TotalItems = total;
			PageSize = pageSize;
		}
	}
}
