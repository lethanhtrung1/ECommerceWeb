using DomainLayer.Entities;
using DomainLayer.Enums;

namespace DomainLayer.Repositories {
	public interface IOrderRepository : IRepository<Order> {
		Task UpdateOrderStatus(Guid id, OrderStatus orderStatus, PaymentStatus paymentStatus);
		Task UpdateShipmentStatus(Guid id, ShipmentStatus shipmentStatus);
	}
}
