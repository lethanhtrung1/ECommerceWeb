namespace DomainLayer.Entities {
	public class ProductSize : BaseEntity {
		public ProductSize() {
			Id = Guid.NewGuid();
		}

		public Guid SizeId { get; set; }
		public Guid ProductId { get; set; }
		public int SizeAvailablity { get; set; }

		public virtual Product? Product { get; set; }
		public virtual Size? Size { get; set; }
	}
}
