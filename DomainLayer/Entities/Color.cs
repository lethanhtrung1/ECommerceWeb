namespace DomainLayer.Entities {
	public class Color : BaseEntity {
		public Color() {
			Id = Guid.NewGuid();
			ProductColors = new List<ProductColor>();
		}

		public string Name { get; set; } = string.Empty;
		public string? HaxValue { get; set; }
		public string? Description { get; set; }

		public virtual ICollection<ProductColor> ProductColors { get; set; }
	}
}
