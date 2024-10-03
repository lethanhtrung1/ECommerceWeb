namespace ApplicationLayer.DTOs.Request.Category {
	public class UpdateCategoryRequestDto {
		public Guid Id { get; set; }
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
		public bool Published { get; set; }
	}
}
