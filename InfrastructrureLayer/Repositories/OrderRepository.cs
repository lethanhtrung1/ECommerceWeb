using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Repositories;
using InfrastructrureLayer.Common;
using InfrastructrureLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace InfrastructrureLayer.Repositories {
	public class OrderRepository : Repository<Order>, IOrderRepository {
		private readonly AppDbContext _dbContext;

		public OrderRepository(AppDbContext dbContext) : base(dbContext) {
			_dbContext = dbContext;
		}

		public async Task UpdateOrderStatus(Guid id, OrderStatus orderStatus, PaymentStatus paymentStatus) {
			var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

			if (order is not null) {
				order.OrderStatus = orderStatus;

				if (order.PaymentType == PaymentType.BankTranfer) {
					order.PaymentStatus = paymentStatus;
				}
			}
		}

		public async Task UpdateShipmentStatus(Guid id, ShipmentStatus shipmentStatus) {
			var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
			if (order is not null) {
				order.ShipmentStatus = shipmentStatus;
			}
		}
	}
}
