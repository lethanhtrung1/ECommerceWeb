using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.Response.Product {
	public class ProductResponseDto {
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public double Price { get; set; }
		public int stock { get; set; }
		public int Sold { get; set; }
		public ProductStatus ProductStatus { get; set; }
		public string? Thumnail { get; set; }
		public bool Published { get; set; }
		//public ProductStoreDto? Store { get; set; }
		//public List<ProductCategoryDto>? Categories { get; set; }
		//public List<ProductImageDto>? Images { get; set; }
		//public List<ProductColorDto>? Colors { get; set; }
		//public List<ProductSizeDto>? Sizes { get; set; }
		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
