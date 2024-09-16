using DomainLayer.Enums;

namespace DomainLayer.Entities {
	public class Payment : BaseEntity {
		public Payment() {
			Id = Guid.NewGuid();
		}

		public string Name { get; set; } = string.Empty;
		public PaymentType PaymentType { get; set; }
	}
}
