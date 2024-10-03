using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Category;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Category;

namespace ApplicationLayer.Interfaces {
	public interface ICategoryService {
		Task<ApiResponse<CategoryResponseDto>> GetCategoryById(Guid categoryId);
		Task<ApiResponse<PagedList<CategoryResponseDto>>> GetCategories(PagingRequest request);
		Task<GeneralResponse> AddCategory(CreateCategoryRequestDto request);
		Task<GeneralResponse> UpdateCategory(UpdateCategoryRequestDto request);
		Task<GeneralResponse> DeleteCategory(Guid categoryId);
		Task<GeneralResponse> DeleteCategories(IEnumerable<Guid> categoryIds);
	}
}
