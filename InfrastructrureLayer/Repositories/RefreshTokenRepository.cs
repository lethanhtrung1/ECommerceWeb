using DomainLayer.Entities.Auth;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository {
		public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
