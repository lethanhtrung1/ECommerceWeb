using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
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
		[HttpGet("roles")]
		public async Task<IActionResult> GetRoles() {
			return Ok(await _userService.GetRolesAsync());
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("users-with-role")]
		public async Task<IActionResult> GetUsersWithRole(PagingRequest request) {
			return Ok(await _userService.GetUsersAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("{userId:Guid}")]
		public async Task<IActionResult> GetUserById(Guid userId) {
			return Ok(await _userService.GetUserByIdAsync(userId));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("change-role")]
		public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequestDto request) {
			return Ok(await _userService.ChangeUserRoleAsync(request));
		}
	}
}
