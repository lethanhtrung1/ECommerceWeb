using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Product;

namespace ApplicationLayer.Interfaces {
	public interface IProductService {
		Task<ApiResponse<ProductResponseDto>> GetProductAsync(Guid productId);
		Task<ApiResponse<PagedList<ProductResponseDto>>> GetProductsAsync(PagingRequest request);
		Task<ApiResponse<ProductResponseDto>> AddAsync(CreateProductRequestDto request);
		Task<ApiResponse<ProductResponseDto>> UpdateAsync(UpdateProductRequestDto request);
		Task<GeneralResponse> DeleteAsync(Guid productId);
	}
}
