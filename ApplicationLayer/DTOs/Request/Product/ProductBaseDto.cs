using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.Request.Product {
	public class ProductBaseDto {
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public int Sold { get; set; } = 0;
		public ProductStatus ProductStatus { get; set; }
		public string? Thumnail { get; set; }
		public Guid BrandId { get; set; }
		public bool Published { get; set; }
		//public List<Guid>? CategoriyIds {  get; set; }
		//public List<Guid>? SizeIds { get; set; }
		//public List<Guid>? ColorIds { get; set; }
		//public List<string>? ProductImages { get; set; }
		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
