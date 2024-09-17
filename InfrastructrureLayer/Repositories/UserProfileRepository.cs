using DomainLayer.Entities.Auth;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository {
		public UserProfileRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
