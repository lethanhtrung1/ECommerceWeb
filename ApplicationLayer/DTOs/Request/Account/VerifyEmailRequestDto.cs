namespace ApplicationLayer.DTOs.Request.Account {
	public class VerifyEmailRequestDto {
		public string Email { get; set; }
		public string Token { get; set; }
	}
}
