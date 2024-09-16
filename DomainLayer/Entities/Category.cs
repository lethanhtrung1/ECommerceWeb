namespace DomainLayer.Entities {
	public class Category : BaseEntity {
		public Category() {
			Id = Guid.NewGuid();
		}

		public string CategoryName { get; set; } = null!;
		public string? Description { get; set; }
		public bool Published { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
	}
}
