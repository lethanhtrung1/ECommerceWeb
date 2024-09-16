using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class CartRepository : Repository<Cart>, ICartRepository {
		public CartRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
