using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class ProductSizeRepository : Repository<ProductSize>, IProductSizeRepository {
		public ProductSizeRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
