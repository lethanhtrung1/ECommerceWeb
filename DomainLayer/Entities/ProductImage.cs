namespace DomainLayer.Entities {
	public partial class ProductImage : BaseEntity {
		public ProductImage() {
			Id = Guid.NewGuid();
		}

		public Guid? ProductId { get; set; }
		public string ImageName { get; set; } = null!;
		public string StoredImage { get; set; } = null!;

		public virtual Product? Product { get; set; }
	}
}
