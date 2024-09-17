namespace DomainLayer.Repositories {
	public interface IUnitOfWork {
		IProductRepository Product { get; }
		ICategoryRepository Category { get; }
		ICartRepository Cart { get; }
		IDiscountRepository Discount { get; }
		INotificationRepository Notification { get; }
		INotificationUserRepository NotificationUser { get; }
		IOrderDetailRepository OrderDetail { get; }
		IOrderRepository Order { get; }
		IPaymentRepository Payment { get; }
		IProductCategoryRepository ProductCategory { get; }
		IProductImageRepository ProductImage { get; }
		IProductReviewRepository ProductReview { get; }
		IProductSizeRepository ProductSize { get; }
		IProductColorRepository ProductColor { get; }
		IShipmentRepository Shipment { get; }
		IStoreRepository Store { get; }
		IUserProfileRepository UserProfile { get; }
		IWishListRepository WishList { get; }
		Task SaveChangesAsync();
	}
}
