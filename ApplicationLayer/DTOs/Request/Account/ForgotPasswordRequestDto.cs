using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Request.Account {
	public class ForgotPasswordRequestDto {
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
