using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.User;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using DomainLayer.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services {
	public class UserService : IUserService {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ILogException _logger;

		public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogException logger) {
			_userManager = userManager;
			_roleManager = roleManager;
			_logger = logger;
		}

		public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDto request) {
			try {
				if ((await _roleManager.FindByNameAsync(request.RoleName)) is null) {
					return new GeneralResponse(false, "Role not found");
				}

				if ((await _userManager.FindByEmailAsync(request.Email)) is null) {
					return new GeneralResponse(false, "User not found");
				}

				var user = await _userManager.FindByEmailAsync(request.Email);

				// Get old role
				var previousRole = (await _userManager.GetRolesAsync(user!)).FirstOrDefault();

				// Remove old role
				var removeOldRoleResult = await _userManager.RemoveFromRoleAsync(user!, previousRole!);
				if (!removeOldRoleResult.Succeeded) {
					return new GeneralResponse(false, "Internal server error occurred");
				}

				// Add new role
				var changeRoleResult = await _userManager.AddToRoleAsync(user!, request.RoleName);
				if (!changeRoleResult.Succeeded) {
					return new GeneralResponse(false, "Internal server error occurred");
				}

				return new GeneralResponse(true, "Role changed");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<IEnumerable<GetRoleDto>> GetRolesAsync() {
			var roles = new List<GetRoleDto>();
			var listRole = await _roleManager.Roles.ToListAsync();
			foreach (var role in listRole) {
				var item = new GetRoleDto {
					RoleName = role.Name,
				};
				roles.Add(item);
			}
			return roles;
		}

		public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(Guid userId) {
			try {
				var user = await _userManager.FindByIdAsync(userId.ToString());

				if (user is null) {
					return new ApiResponse<UserResponseDto>(false, "User not found");
				}

				var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
				var roleInfo = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name!.ToLower() == role!.ToLower());
				var userWithRole = new UserResponseDto {
					Id = user.Id,
					Name = user.Name,
					Email = user.Email,
					RoleId = roleInfo!.Id,
					RoleName = roleInfo.Name,
				};

				return new ApiResponse<UserResponseDto>(userWithRole, true, "");
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				throw;
			}
		}

		public async Task<ApiResponse<PagedList<UserResponseDto>>> GetUsersAsync(PagingRequest request) {
			try {
				var users = await _userManager.Users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

				if (users is null || users.Count == 0) {
					return new ApiResponse<PagedList<UserResponseDto>>(false, "Users not found");
				}

				var usersWithRole = new List<UserResponseDto>();
				foreach (var user in users) {
					var getUserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
					var getRoleInfo = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name!.ToLower() == getUserRole!.ToLower());
					usersWithRole.Add(new UserResponseDto {
						Id = user.Id,
						Name = user.Name,
						Email = user.Email,
						RoleId = getRoleInfo!.Id,
						RoleName = getRoleInfo.Name,
					});
				}

				int totalUser = await _userManager.Users.CountAsync();

				return new ApiResponse<PagedList<UserResponseDto>>(
						new PagedList<UserResponseDto>(
								usersWithRole, request.PageNumber, request.PageSize, totalUser
							),
						true,
						""
					);
			} catch (Exception ex) {
				// Log the original exception
				_logger.LogExceptions(ex);
				return new ApiResponse<PagedList<UserResponseDto>>(false, ex.Message);
			}
		}
	}
}
