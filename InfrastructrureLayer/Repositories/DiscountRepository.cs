using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class DiscountRepository : Repository<Discount>, IDiscountRepository {
		public DiscountRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
