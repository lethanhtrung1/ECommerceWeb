using DomainLayer.Entities;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;

namespace InfrastructrureLayer.Repositories {
	public class ShipmentRepository : Repository<Shipment>, IShipmentRepository {
		public ShipmentRepository(AppDbContext dbContext) : base(dbContext) {
		}
	}
}
