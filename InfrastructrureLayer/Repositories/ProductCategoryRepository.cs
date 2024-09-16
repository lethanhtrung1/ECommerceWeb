using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository {
		public ProductCategoryRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
