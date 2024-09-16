using DomainLayer.Enums;

namespace DomainLayer.Entities {
	public class Shipment : BaseEntity {
		public Shipment() {
			Id = Guid.NewGuid();
		}

		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public bool IsActive { get; set; }
		public ShipmentType ShipmentType { get; set; }
	}
}
