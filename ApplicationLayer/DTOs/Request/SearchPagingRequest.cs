using ApplicationLayer.DTOs.Pagination;

namespace ApplicationLayer.DTOs.Request {
	public class SearchPagingRequest : PagingRequest {
		public string? Keyword { get; set; }
	}
}
