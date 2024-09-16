using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class NotificationRepository : Repository<Notification>, INotificationRepository {
		public NotificationRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
