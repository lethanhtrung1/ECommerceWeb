namespace ApplicationLayer.DTOs.Response.Product {
	public class ProductStoreDto {
		public Guid Id { get; set; }
		public string? StoreName { get; set; }
		public string? StoreAddress { get; set; }
		public string? ContactNumber { get; set; }
		public string? StoreLogo { get; set; }
	}
}
