using DomainLayer.Repositories;
using InfrastructrureLayer.Data;
using InfrastructrureLayer.Repositories;

namespace InfrastructrureLayer.Common {
	public class UnitOfWork : IUnitOfWork {
		private readonly AppDbContext _dbContext;
		public IProductRepository Product { get; set; }
		public ICategoryRepository Category { get; set; }
		public ICartRepository Cart { get; set; }
		public IDiscountRepository Discount { get; set; }
		public INotificationRepository Notification { get; set; }
		public INotificationUserRepository NotificationUser { get; set; }
		public IOrderDetailRepository OrderDetail { get; set; }
		public IOrderRepository Order {  get; set; }
		public IPaymentRepository Payment { get; set; }
		public IProductCategoryRepository ProductCategory { get; set; }
		public IProductColorRepository ProductColor { get; set; }
		public IProductImageRepository ProductImage { get; set; }
		public IProductSizeRepository ProductSize { get; set; }
		public IProductReviewRepository ProductReview { get; set; }
		public IShipmentRepository Shipment { get; set; }
		public IStoreRepository Store { get; set; }
		public IBrandRepository Brand { get; set; }
		public IUserProfileRepository UserProfile { get; set; }
		public IWishListRepository WishList { get; set; }
		public IRefreshTokenRepository RefreshToken { get; set; }

		public UnitOfWork(AppDbContext dbContext) {
			_dbContext = dbContext;
			Product = new ProductRepository(_dbContext);
			Category = new CategoryRepository(_dbContext);
			Cart = new CartRepository(_dbContext);
			Discount = new DiscountRepository(_dbContext);
			Notification = new NotificationRepository(_dbContext);
			NotificationUser = new NotificationUserRepository(_dbContext);
			Order = new OrderRepository(_dbContext);
			OrderDetail = new OrderDetailRepository(_dbContext);
			Payment = new PaymentRepository(_dbContext);
			ProductCategory = new ProductCategoryRepository(_dbContext);
			ProductColor = new ProductColorRepository(_dbContext);
			ProductImage = new ProductImageRepository(_dbContext);
			ProductSize = new ProductSizeRepository(_dbContext);
			ProductReview = new ProductReviewRepository(_dbContext);
			Shipment = new ShipmentRepository(_dbContext);
			Store = new StoreRepository(_dbContext);
			Brand = new BrandRepository(_dbContext);
			UserProfile = new UserProfileRepository(_dbContext);
			WishList = new WishListRepository(_dbContext);
			RefreshToken = new RefreshTokenRepository(_dbContext);
		}

		public async Task SaveChangesAsync() {
			await _dbContext.SaveChangesAsync();
		}
	}
}
