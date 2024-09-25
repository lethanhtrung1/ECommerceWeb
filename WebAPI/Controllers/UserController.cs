using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.User;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApplicationLayer.Common.Constant;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {
		private readonly IUserService _userService;

		public UserController(IUserService userService) {
			_userService = userService;
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("user/role")]
		public async Task<ActionResult<IEnumerable<GetRoleDto>>> GetRoles() {
			return Ok(await _userService.GetRolesAsync());
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("user/users-with-role")]
		public async Task<ActionResult<ApiResponse<PagedList<UserResponseDto>>>> GetUsersWithRole(PagingRequest request) {
			return Ok(await _userService.GetUsersAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("user/change-role")]
		public async Task<ActionResult<GeneralResponse>> ChangeUserRole(ChangeUserRoleRequestDto request) {
			return Ok(await _userService.ChangeUserRoleAsync(request));
		}
	}
}
