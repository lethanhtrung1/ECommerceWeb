namespace ApplicationLayer.DTOs.Response.Category {
	public class CategoryResponseDto {
		public Guid Id { get; set; }
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
		public bool Published { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
