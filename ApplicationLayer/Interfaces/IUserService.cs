using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.User;

namespace ApplicationLayer.Interfaces {
	public interface IUserService {
		Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDto request);
		Task<IEnumerable<GetRoleDto>> GetRolesAsync();
		Task<ApiResponse<PagedList<UserResponseDto>>> GetUsersAsync(PagingRequest request);
		Task<ApiResponse<UserResponseDto>> GetUserById(Guid userId);
	}
}
