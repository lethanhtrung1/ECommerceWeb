using DomainLayer.Entities.Auth;

namespace DomainLayer.Entities {
	public class Store : BaseEntity {
		public Store() {
			Id = Guid.NewGuid();
			Products = new HashSet<Product>();
		}

		public Guid SellerId { get; set; }
		public string? StoreName { get; set; }
		public string? Description { get; set; }
		public string StoreAddress { get; set; } = null!;
		public string ContactNumber { get; set; } = null!;
		public string? StoreLogo { get; set; }
		public Guid ProvinceId { get; set; }
		
		public virtual Province? Province { get; set; }
		public virtual ApplicationUser? Seller { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}
