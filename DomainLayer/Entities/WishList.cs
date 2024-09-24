using DomainLayer.Entities.Auth;

namespace DomainLayer.Entities {
	public class WishList : BaseEntity {
		public WishList() {
			Id = Guid.NewGuid();
		}

		public string? UserId { get; set; }
		public Guid? ProductId { get; set; }

		public virtual Product? Product { get; set; }
		public virtual ApplicationUser? User { get; set; }
	}
}
