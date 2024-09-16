using DomainLayer.Entities.Auth;

namespace DomainLayer.Entities {
	public class Province : BaseEntity {
		public Province() {
			Id = Guid.NewGuid();
			Districts = new HashSet<District>();
			UserInfomations = new HashSet<UserProfile>();
		}

		public string ProvinceName { get; set; } = null!;

		public virtual ICollection<District> Districts { get; set; }
		public virtual ICollection<UserProfile> UserInfomations { get; set; }
	}
}
