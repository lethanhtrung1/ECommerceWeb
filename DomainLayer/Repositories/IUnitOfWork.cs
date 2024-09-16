namespace DomainLayer.Repositories {
	public interface IUnitOfWork {
		IProductRepository Product { get; }
		ICategoryRepository Category { get; }
		ICartRepository Cart { get; }
		IDiscountRepository Discount { get; }
		Task SaveChangesAsync();
	}
}
