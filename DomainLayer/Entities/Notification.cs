namespace DomainLayer.Entities {
	public class Notification : BaseEntity {
		public Notification() {
			Id = Guid.NewGuid();
			NotificationUsers = new HashSet<NotificationUser>();
		}

		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Content { get; set; }
		public DateTime SeedTime { get; set; }
		public bool Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
	}
}
