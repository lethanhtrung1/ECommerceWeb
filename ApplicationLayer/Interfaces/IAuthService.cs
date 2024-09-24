using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Account;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Account;

namespace ApplicationLayer.Interfaces
{
    public interface IAuthService {
		Task<GeneralResponse> CreateRoleAsync(CreateRoleRequestDto request);
		Task<GeneralResponse> CreateAccountAsync(RegistrationRequestDto request);
		Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
		Task<AuthResponseDto> RefreshTokenAsync(TokenRequest request);
		Task<GeneralResponse> ChangePasswordAsync(ChangePasswordRequestDto request);

		Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDto request);
		Task<IEnumerable<GetRoleDto>> GetRolesAsync();
		Task<ApiResponse<PagedList<GetUserResponseDto>>> GetUsersAsync(PagingRequest request);
	}
}
