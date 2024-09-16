using DomainLayer.Enums;

namespace DomainLayer.Entities {
	public class Discount : BaseEntity {
		public Discount() {
			Id = Guid.NewGuid();
		}

		public string? Name { get; set; }
		public DiscountType DiscountType { get; set; }
		public bool UsePercentage { get; set; }
		public decimal DiscountPercentage { get; set; }
		public decimal DiscountAmount { get; set; }
		public DateTime? AvailableFrom { get; set; }
		public DateTime? AvailableTo { get; set; }
		public bool IsActive { get; set; }
		public decimal? MaximumDiscountAmount { get; set; }
		public int? MaximumDiscountedQuantity { get; set; }
		public bool RequiresCouponCode { get; set; }
		public string? CouponCode { get; set; }
	}
}
