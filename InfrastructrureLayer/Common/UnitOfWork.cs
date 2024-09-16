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

		public UnitOfWork(AppDbContext dbContext) {
			_dbContext = dbContext;
			Product = new ProductRepository(_dbContext);
			Category = new CategoryRepository(_dbContext);
			Cart = new CartRepository(_dbContext);
			Discount = new DiscountRepository(_dbContext);
		}

		public async Task SaveChangesAsync() {
			await _dbContext.SaveChangesAsync();
		}
	}
}
