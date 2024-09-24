using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.Response.Product {
	public class ProductResponseDto {
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; } = 0;
		public int Sold { get; set; } = 0;
		public ProductStatus ProductStatus { get; set; }
		public string? ProductImage { get; set; }

		public List<ProductCategoryDto> ProductCategories { get; set; }
		public ProductStoreDto Store { get; set; }

		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
