using DomainLayer.Entities.Auth;
using DomainLayer.Enums;

namespace DomainLayer.Entities {
	public class Order : BaseEntity {
		public Order() {
			Id = Guid.NewGuid();
			Details = new HashSet<OrderDetail>();
		}

		public Guid UserId { get; set; }
		public string? Note { get; set; }
		public decimal BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public int? DiscountValue { get; set; }
		public string Address { get; set; } = string.Empty;
		public OrderStatus OrderStatus { get; set; }
		public PaymentType PaymentType { get; set; }
		public PaymentStatus PaymentStatus { get; set; }
		public ShipmentType ShipmentType { get; set; }
		public ShipmentStatus ShipmentStatus { get; set; }
		public bool IsReceived { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ReceivedDate { get; set; }

		public virtual ApplicationUser? User { get; set; }
		public virtual ICollection<OrderDetail> Details { get; set; }
	}
}
