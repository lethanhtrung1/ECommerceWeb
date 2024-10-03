namespace DomainLayer.Entities.Interfaces {
	public interface IDateTracking {
		DateTime CreatedDate { get; set; }
		DateTime LastModifiedDate { get; set; }
	}
}
