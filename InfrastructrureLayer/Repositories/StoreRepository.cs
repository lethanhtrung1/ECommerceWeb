using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class StoreRepository : Repository<Store>, IStoreRepository {
		public StoreRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
