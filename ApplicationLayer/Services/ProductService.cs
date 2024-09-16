using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Product;
using ApplicationLayer.Interfaces;
using DomainLayer.Repositories;

namespace ApplicationLayer.Services {
	public class ProductService : IProduct {
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public Task<ApiResponse<ProductResponseDto>> AddAsync(CreateProductRequestDto request) {
			throw new NotImplementedException();
		}
	}
}
