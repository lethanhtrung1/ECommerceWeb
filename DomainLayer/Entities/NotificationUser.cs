using DomainLayer.Entities.Auth;

namespace DomainLayer.Entities {
	public class NotificationUser : BaseEntity {
		public NotificationUser() {
			Id = Guid.NewGuid();
		}

		public Guid NotificationId { get; set; }
		public Guid UserId { get; set; }
		public bool Seen { get; set; }
		public bool Status { get; set; }
		public virtual Notification? Notification { get; set; }
		public virtual ApplicationUser? User { get; set; }
	}
}
