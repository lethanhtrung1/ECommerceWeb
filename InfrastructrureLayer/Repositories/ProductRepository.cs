using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository {
		public ProductRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
