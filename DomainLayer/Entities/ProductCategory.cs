namespace DomainLayer.Entities {
	public class ProductCategory : BaseEntity {
		public ProductCategory() {
			Id = Guid.NewGuid();
		}

		public Guid ProductId { get; set; }
		public Guid CategoryId { get; set; }
		public virtual Product? Product { get; set; }
		public virtual Category? Category { get; set; }
	}
}
