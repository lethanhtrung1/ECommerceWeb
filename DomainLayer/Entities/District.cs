namespace DomainLayer.Entities {
	public class District : BaseEntity {
		public District() {
			Id = Guid.NewGuid();
			Wards = new HashSet<Ward>();
		}

		public string DistrictName { get; set; } = null!;
		public Guid ProvinceId { get; set; }

		public virtual Province? Province { get; set; }
		public virtual ICollection<Ward> Wards { get; set; }
	}
}
