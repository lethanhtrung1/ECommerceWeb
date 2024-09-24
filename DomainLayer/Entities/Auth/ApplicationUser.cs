using Microsoft.AspNetCore.Identity;

namespace DomainLayer.Entities.Auth {
	public class ApplicationUser : IdentityUser {
		public ApplicationUser() {
			Carts = new HashSet<Cart>();
			WishLists = new HashSet<WishList>();
			NotificationUsers = new HashSet<NotificationUser>();
		}

		public string? Name { get; set; }

		public virtual UserProfile? UserProfile { get; set; }
		public virtual ICollection<Cart> Carts { get; set; }
		public virtual ICollection<Order>? Orders { get; set; }
		public virtual ICollection<Store>? Stores { get; set; }
		public virtual ICollection<WishList> WishLists { get; set; }
		public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
	}
}
