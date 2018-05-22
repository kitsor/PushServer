namespace KitsorLab.PushServer.API.Model
{
	using System.ComponentModel.DataAnnotations;

	public class PaginatedRequest
	{
		[Range(1, int.MaxValue)]
		public int Page { get; set; }
		[Range(1, int.MaxValue)]
		public int EntriesPerPage { get; set; }

		public PaginatedRequest()
		{
			Page = 1;
			EntriesPerPage = 20;
		}

		public int GetLimit() => EntriesPerPage;
		public int GetOffset() => (Page - 1) * EntriesPerPage;
	}
}
