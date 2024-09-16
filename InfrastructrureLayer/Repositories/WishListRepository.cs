using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class WishListRepository : Repository<WishList>, IWishListRepository {
		public WishListRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
