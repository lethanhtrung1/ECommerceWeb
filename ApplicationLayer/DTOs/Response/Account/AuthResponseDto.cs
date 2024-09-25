namespace ApplicationLayer.DTOs.Response.Account {
	public class AuthResponseDto {
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		//public string Token { get; set; }
		//public string RefreshToken { get; set; }
		public bool Is2FactorRequired { get; set; }
		public string? Provider { get; set; }

		public AuthResponseDto(bool isSuccess = false, string message = null!) {
			IsSuccess = isSuccess;
			Message = message;
		}
	}
}
