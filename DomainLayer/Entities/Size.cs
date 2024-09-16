namespace DomainLayer.Entities {
	public class Size : BaseEntity {
		public Size() {
			Id = Guid.NewGuid();
			ProductSizes = new List<ProductSize>();
		}

		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public virtual ICollection<ProductSize> ProductSizes { get; set; }
	}
}
