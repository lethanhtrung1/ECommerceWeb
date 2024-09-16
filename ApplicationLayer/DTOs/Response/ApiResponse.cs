namespace ApplicationLayer.DTOs.Response {
	public class ApiResponse<T> {
		public T? Data { get; set; }
		public bool IsSuccess { get; set; } = true;
		public string Message { get; set; } = string.Empty;

		// Constructor for a Successful Response
		public ApiResponse(T data, bool isSuccess, string message) {
			Data = data;
			IsSuccess = isSuccess;
			Message = message;
		}

		// Constructor for an Error Response
		public ApiResponse(bool isSuccess, string message) {
			IsSuccess = isSuccess;
			Message = message;
			Data = default(T);
		}
	}
}
