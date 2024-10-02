using DomainLayer.Enums;

namespace DomainLayer.Entities {
	public class Product : BaseEntity {
		public Product() {
			Id = Guid.NewGuid();
			Carts = new List<Cart>();
			ProductImages = new List<ProductImage>();
			WishLists = new List<WishList>();
			ProductReviews = new List<ProductReview>();
			ProductSizes = new List<ProductSize>();
			ProductColors = new List<ProductColor>();
			ProductCategories = new List<ProductCategory>();
		}

		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; } = 0;
		public int Sold { get; set; } = 0;
		public ProductStatus ProductStatus { get; set; }
		public string? Thumnail { get; set; }
		//public Guid? CategoryId { get; set; }
		public Guid? BrandId { get; set; }
		public string? CreatedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public virtual Brand? Brand { get; set; }
		public virtual Category? Category { get; set; }
		public virtual ICollection<Cart> Carts { get; set; }
		public virtual ICollection<ProductImage> ProductImages { get; set; }
		public virtual ICollection<WishList> WishLists { get; set; }
		public virtual ICollection<ProductReview> ProductReviews { get; set; }
		public virtual ICollection<ProductSize> ProductSizes { get; set; }
		public virtual ICollection<ProductColor> ProductColors { get; set; }
		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
		public virtual ICollection<ProductCategory> ProductCategories { get; set; }
	}
}
