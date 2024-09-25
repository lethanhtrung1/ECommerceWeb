using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Request.Account {
	public class TwoFactorRequestDto {
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Provider { get; set; }
		[Required]
		public string? Token { get; set; }
	}
}
