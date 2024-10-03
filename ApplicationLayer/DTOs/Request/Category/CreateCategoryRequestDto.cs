namespace ApplicationLayer.DTOs.Request.Category {
	public class CreateCategoryRequestDto {
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
		public bool Published { get; set; }
	}
}
