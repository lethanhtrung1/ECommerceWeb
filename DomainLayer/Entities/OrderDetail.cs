namespace DomainLayer.Entities {
	public class OrderDetail : BaseEntity {
		public OrderDetail() {
			Id = Guid.NewGuid();
		}

		public Guid OrderId { get; set; }
		public Guid ProductId { get; set; }
		public int Amount { get; set; }
		public decimal TotalPrice { get; set; }
		public string? Size { get; set; }
		public string? Color { get; set; }

		public virtual Order? Order { get; set; }
		public virtual Product? Product { get; set; }
	}
}
