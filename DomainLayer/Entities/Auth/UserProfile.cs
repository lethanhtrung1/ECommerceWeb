namespace DomainLayer.Entities.Auth {
	public class UserProfile : BaseEntity {
		public UserProfile() {
			Id = Guid.NewGuid();
		}

		public string UserId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? ContactNumber { get; set; }
		public Guid? ProvinceId { get; set; }
		public string? Avatar {  get; set; }

		public virtual ApplicationUser? ApplicationUser { get; set; }
		public virtual Province? Province { get; set; }
	}
}
