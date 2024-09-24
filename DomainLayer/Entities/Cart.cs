using DomainLayer.Entities.Auth;

namespace DomainLayer.Entities {
	public class Cart : BaseEntity {
		public Cart() {
			Id = Guid.NewGuid();
		}

		public string UserId { get; set; }
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public virtual ApplicationUser? User { get; set; }
		public virtual Product? Product { get; set; }
	}
}
