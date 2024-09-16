namespace DomainLayer.Entities {
	public class ProductColor : BaseEntity {
		public ProductColor() {
			Id = Guid.NewGuid();
		}

		public Guid ProductId { get; set; }
		public Guid ColorId { get; set; }
		public int ColorAvailability { get; set; }

		public virtual Product? Product { get; set; }
		public virtual Color? Color { get; set; }
	}
}
