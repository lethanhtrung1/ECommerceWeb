using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApplicationLayer.Common.Constant;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase {
		private readonly IAuthService _authService;

		public AccountController(IAuthService authService) {
			_authService = authService;
		}

		[HttpPost("identity/register")]
		public async Task<ActionResult<GeneralResponse>> Register(RegistrationRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.CreateAccountAsync(request));
		}

		[HttpPost("identity/login")]
		public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.LoginAsync(request));
		}

		[HttpPost("identity/refresh-token")]
		public async Task<ActionResult<AuthResponseDto>> RefreshToken(TokenRequest request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.RefreshTokenAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("identity/role")]
		public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.CreateRoleAsync(request));
		}

		[Authorize]
		[HttpPost("identity/change-password")]
		public async Task<ActionResult<GeneralResponse>> ChangePassword(ChangePasswordRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.ChangePasswordAsync(request));
		}

		[HttpPost("identity/email-confirmation")]
		public async Task<ActionResult<GeneralResponse>> EmailConfirmation(VerifyEmailRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.EmailConfirmation(request));
		}

		[HttpPost("identity/forgot-password")]
		public async Task<ActionResult<GeneralResponse>> ForgotPassword(ForgotPasswordRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.ForgotPassword(request));
		}

		[HttpPost("identity/reset-password")]
		public async Task<ActionResult<GeneralResponse>> ResetPassword(ResetPasswordRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.ResetPassword(request));
		}

		[HttpPost("identity/enable-2fa")]
		public async Task<ActionResult<GeneralResponse>> EnableTwoFactor(string email) {
			return Ok(await _authService.EnableTwoFactor(email));
		}

		[HttpPost("identity/disable-2fa")]
		public async Task<ActionResult<GeneralResponse>> DisableTwoFactor(string email) {
			return Ok(await _authService.DisableTwoFactor(email));
		}
	}
}
