using DomainLayer.Entities;
using DomainLayer.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructrureLayer.Data {
	public class AppDbContext : IdentityDbContext<ApplicationUser> {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		#region DbSet

		public DbSet<Product> Products { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers {  get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<Discount> Discounts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Shipment> Shipments { get; set; }
		public DbSet<WishList> WishLists { get; set; }
		public DbSet<ProductColor> ProductColors { get; set; }
		public DbSet<ProductSize> ProductSizes { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductReview> ProductReviews { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<Store> Stores { get; set; }
		public DbSet<Province> Provinces { get; set; }
		public DbSet<District> Districts { get; set; }
		public DbSet<Ward> Wards { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<NotificationUser> NotificationUsers { get; set; }

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserProfile>(entity => {
				entity.HasOne(d => d.ApplicationUser)
					.WithOne(p => p.UserProfile)
					.HasForeignKey<UserProfile>(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.Province)
					.WithMany(p => p.UserInfomations)
					.HasForeignKey(d => d.ProvinceId);
			});

			modelBuilder.Entity<Category>(entity => {
				entity.Property(e => e.CategoryName).HasMaxLength(100);
				entity.Property(e => e.Description).HasMaxLength(1000);

				entity.HasMany(d => d.ProductCategories)
					.WithOne(p => p.Category)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<ProductCategory>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductCategories)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Category)
					.WithMany(p => p.ProductCategories)
					.HasForeignKey(d => d.CategoryId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Product>(entity => {
				entity.Property(e => e.Name).HasMaxLength(100);
				entity.Property(e => e.Description).HasMaxLength(1000);

				entity.HasMany(d => d.ProductCategories)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.ProductImages)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.ProductSizes)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.ProductColors)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.ProductReviews)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.OrderDetails)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasMany(d => d.Carts)
					.WithOne(p => p.Product)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Cart>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.Carts)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.User)
					.WithMany(p => p.Carts)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Order>(entity => {
				entity.HasOne(d => d.User)
					.WithMany(p => p.Orders)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<OrderDetail>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.OrderDetails)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(d => d.Order)
					.WithMany(p => p.Details)
					.HasForeignKey(d => d.OrderId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ProductColor>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductColors)
					.HasForeignKey(d => d.ProductId);

				entity.HasOne(d => d.Color)
					.WithMany(p => p.ProductColors)
					.HasForeignKey(d => d.ColorId);
			});

			modelBuilder.Entity<ProductSize>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductSizes)
					.HasForeignKey(d => d.ProductId);

				entity.HasOne(d => d.Size)
					.WithMany(p => p.ProductSizes)
					.HasForeignKey(d => d.SizeId);
			});

			modelBuilder.Entity<ProductImage>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductImages)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<ProductReview>(entity => {
				entity.HasOne(d => d.Product)
					.WithMany(p => p.ProductReviews)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Color>(entity => {
				entity.HasMany(d => d.ProductColors)
					.WithOne(p => p.Color);
			});

			modelBuilder.Entity<Size>(entity => {
				entity.HasMany(d => d.ProductSizes)
					.WithOne(p => p.Size);
			});

			modelBuilder.Entity<WishList>(entity => {
				entity.HasOne(d => d.User)
					.WithMany(p => p.WishLists)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.Product)
					.WithMany(p => p.WishLists)
					.HasForeignKey(d => d.ProductId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Discount>(entity => {
				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<Province>(entity => {
				entity.HasMany(d => d.Districts)
					.WithOne(p => p.Province);

				entity.HasMany(d => d.UserInfomations)
					.WithOne(p => p.Province);
			});

			modelBuilder.Entity<District>(entity => {
				entity.HasOne(d => d.Province)
					.WithMany(p => p.Districts)
					.HasForeignKey(d => d.ProvinceId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasMany(d => d.Wards)
					.WithOne(p => p.District);
			});

			modelBuilder.Entity<Ward>(entity => {
				entity.HasOne(d => d.District)
					.WithMany(p => p.Wards)
					.HasForeignKey(d => d.DistrictId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<NotificationUser>(entity => {
				entity.HasOne(d => d.Notification)
					.WithMany(p => p.NotificationUsers)
					.HasForeignKey(d => d.NotificationId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(d => d.User)
					.WithMany(p => p.NotificationUsers)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
