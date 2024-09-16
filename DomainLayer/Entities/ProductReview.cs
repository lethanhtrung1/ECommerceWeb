namespace DomainLayer.Entities {
	public class ProductReview : BaseEntity {
		public ProductReview() {
			Id = Guid.NewGuid();
		}

		public Guid UserId { get; set; }
		public Guid ProductId { get; set; }
		public Guid StoreId { get; set; }
		public string Title { get; set; } = null!;
		public string? Comment { get; set; }
		public int Rating { get; set; }
		public bool IsHidden { get; set; }
		public DateTime CreatedAt { get; set; }

		public virtual Product? Product { get; set; }
	}
}
