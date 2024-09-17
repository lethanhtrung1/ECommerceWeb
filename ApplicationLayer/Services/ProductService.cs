using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Product;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using DomainLayer.Repositories;

namespace ApplicationLayer.Services {
	public class ProductService : IProduct {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogException _logger;

		public ProductService(IUnitOfWork unitOfWork, ILogException logger) {
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public Task<ApiResponse<ProductResponseDto>> AddAsync(CreateProductRequestDto request) {
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Guid productId) {
			throw new NotImplementedException();
		}

		public Task<ApiResponse<ProductResponseDto>> GetProductAsync(Guid productId) {
			throw new NotImplementedException();
		}

		public Task<ApiResponse<PagedList<ProductResponseDto>>> GetProductsAsync(PagingRequest request) {
			throw new NotImplementedException();
		}

		public Task<ApiResponse<ProductResponseDto>> UpdateAsync(UpdateProductRequestDto request) {
			throw new NotImplementedException();
		}
	}
}
