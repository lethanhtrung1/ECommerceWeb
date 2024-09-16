namespace ApplicationLayer.DTOs.Response.Account {
	public class AuthResponseDto {
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public string Token { get; set; }
		public string RefreshToken { get; set; }

		public AuthResponseDto(bool isSuccess = false, string message = null!, string token = null!, string refreshToken = null!) {
			IsSuccess = isSuccess;
			Message = message;
			Token = token;
			RefreshToken = refreshToken;
		}
	}
}
