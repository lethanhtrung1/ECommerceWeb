using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class NotificationUserRepository : Repository<NotificationUser>, INotificationUserRepository {
		public NotificationUserRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
