namespace ApplicationLayer.DTOs.Request.Account {
	public class ChangePasswordRequestDto {
		public string Email { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
