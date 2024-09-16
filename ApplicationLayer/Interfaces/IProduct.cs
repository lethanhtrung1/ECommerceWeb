using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Product;

namespace ApplicationLayer.Interfaces {
	public interface IProduct {
		Task<ApiResponse<ProductResponseDto>> AddAsync(CreateProductRequestDto request);
	}
}
