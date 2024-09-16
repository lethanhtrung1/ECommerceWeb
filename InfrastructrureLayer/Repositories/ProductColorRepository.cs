using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class ProductColorRepository : Repository<ProductColor>, IProductColorRepository {
		public ProductColorRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
