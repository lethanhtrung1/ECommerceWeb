namespace DomainLayer.Entities {
	public class Ward : BaseEntity {
		public Ward() {
			Id = Guid.NewGuid();
		}

		public string WardName { get; set; } = null!;
		public Guid DistrictId { get; set; }

		public virtual District? District { get; set; }
	}
}
