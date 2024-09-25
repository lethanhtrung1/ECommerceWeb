using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;

namespace ApplicationLayer.Interfaces {
	public interface IAuthService {
		Task<GeneralResponse> CreateRoleAsync(CreateRoleRequestDto request);
		Task<GeneralResponse> CreateAccountAsync(RegistrationRequestDto request);
		Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
		Task<AuthResponseDto> RefreshTokenAsync(TokenRequest request);
		Task<GeneralResponse> ChangePasswordAsync(ChangePasswordRequestDto request);
		Task<GeneralResponse> EmailConfirmation(VerifyEmailRequestDto request);
		Task<GeneralResponse> ForgotPassword(ForgotPasswordRequestDto request);
		Task<GeneralResponse> ResetPassword(ResetPasswordRequestDto request);
		Task<GeneralResponse> EnableTwoFactor(string email);
		Task<GeneralResponse> DisableTwoFactor(string email);
	}
}
