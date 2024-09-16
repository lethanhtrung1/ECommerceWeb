using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApplicationLayer.Common.Constant;

namespace WebAPI.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase {
		private readonly IAuthService _authService;

		public AccountController(IAuthService authService) {
			_authService = authService;
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("setting/create-admin")]
		public async Task<ActionResult> CreateAdmin() {
			await _authService.CreateAdmin();
			return Ok();
		}


		[HttpPost("identity/register")]
		public async Task<ActionResult<GeneralResponse>> Registration(RegistrationRequestDto request) {
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
		[HttpGet("identity/role/get-all")]
		public async Task<ActionResult<IEnumerable<GetRoleDto>>> GetRoles() {
			return Ok(await _authService.GetRolesAsync());
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("identity/role/create")]
		public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleRequestDto request) {
			if (!ModelState.IsValid) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _authService.CreateRoleAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("identity/users-with-role")]
		public async Task<ActionResult<ApiResponse<PagedList<GetUserResponseDto>>>> GetUsersWithRole(PagingRequest request) {
			return Ok(await _authService.GetUsersAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("identity/change-role")]
		public async Task<ActionResult<GeneralResponse>> ChangeUserRole(ChangeUserRoleRequestDto request) {
			return Ok(await _authService.ChangeUserRoleAsync(request));
		}

		[Authorize]
		[HttpPost("identity/change-password")]
		public async Task<ActionResult<GeneralResponse>> ChangePassword(ChangePasswordRequestDto request) {
			return Ok(await _authService.ChangePasswordAsync(request));
		}
	}
}
