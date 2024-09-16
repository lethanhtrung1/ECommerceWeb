using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Request.Product {
	public class ProductBaseDto {
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; } = 0;
		public int Sold { get; set; } = 0;
		public string Brand { get; set; } = default!;
		public string? ProductImage { get; set; }
		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; }

		public Guid CategoryId {  get; set; }
	}
}
