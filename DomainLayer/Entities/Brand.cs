namespace DomainLayer.Entities {
	public class Brand : BaseEntity {
		public Brand() {
			Id = Guid.NewGuid();
			Products = new List<Product>();
		}

		public string? Name { get; set; }
		public string? Description { get; set; }

		public virtual ICollection<Product> Products { get; set; }
	}
}
