using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Product;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApplicationLayer.Services {
	public class ProductService : IProductService {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogException _logger;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProductService(IUnitOfWork unitOfWork, ILogException logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) {
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ApiResponse<ProductResponseDto>> AddAsync(CreateProductRequestDto request) {
			try {
				if (request is null) {
					return new ApiResponse<ProductResponseDto>(false, "Invalid payload");
				}

				var productToDb = _mapper.Map<Product>(request);
				productToDb.CreatedAt = DateTime.Now;
				productToDb.CreatedBy = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

				await _unitOfWork.Product.BeginTransactionAsync();

				await _unitOfWork.Product.AddAsync(productToDb);
				await _unitOfWork.SaveChangesAsync();

				await _unitOfWork.Product.EndTransactionAsync();

				var productFromDb = await _unitOfWork.Product.GetAsync(x => x.Name == productToDb.Name);
				var result = _mapper.Map<ProductResponseDto>(productFromDb);

				return new ApiResponse<ProductResponseDto>(result, true, "Added new product successful");
			} catch (Exception ex) {
				await _unitOfWork.Product.RollBackTransactionAsync();
				_logger.LogExceptions(ex);
				return new ApiResponse<ProductResponseDto>(false, $"Error adding new product: {ex.Message}");
			}
		}

		public async Task<GeneralResponse> DeleteAsync(Guid productId) {
			try {
				var productFromDb = await _unitOfWork.Product.GetAsync(x => x.Id == productId);

				if (productFromDb is null) {
					return new GeneralResponse(false, "Product not found");
				}

				await _unitOfWork.Product.RemoveAsync(productFromDb);
				await _unitOfWork.SaveChangesAsync();

				return new GeneralResponse(true, "Deleted successful");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, $"Error deleting product with Id: {productId}: {ex.Message}");
			}
		}

		public async Task<GeneralResponse> DeleteListAsync(IEnumerable<Guid> productIds) {
			try {
				var products = await _unitOfWork.Product.GetListAsync(x => productIds.Contains(x.Id));

				if (products is null || products.Count() == 0) {
					return new GeneralResponse(false, "No record avaliable");
				}

				await _unitOfWork.Product.RemoveRangeAsync(products);
				await _unitOfWork.SaveChangesAsync();

				return new GeneralResponse(true, "Deleted successfully");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<ApiResponse<ProductResponseDto>> GetProductAsync(Guid productId) {
			try {
				var product = await _unitOfWork.Product.GetAsync(x => x.Id == productId);

				if (product is null) {
					return new ApiResponse<ProductResponseDto>(false, $"Product with the Id: {productId} does not exist");
				}

				var result = _mapper.Map<ProductResponseDto>(product);

				return new ApiResponse<ProductResponseDto>(result, true, "");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new ApiResponse<ProductResponseDto>(false, $"Internal Server Error: {ex.Message}");
			}
		}

		public async Task<ApiResponse<PagedList<ProductResponseDto>>> GetProductsAsync(PagingRequest request) {
			try {
				var products = await _unitOfWork.Product.GetListAsync();
				var productsPageList = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
				int totalRecord = products.Count();

				if (productsPageList is null || productsPageList.Count() == 0) {
					return new ApiResponse<PagedList<ProductResponseDto>>(false, "No products available");
				}

				var result = _mapper.Map<List<ProductResponseDto>>(products);

				return new ApiResponse<PagedList<ProductResponseDto>>(
						new PagedList<ProductResponseDto>(
							result,
							request.PageNumber,
							request.PageSize,
							totalRecord
						),
						true,
						""
					);
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new ApiResponse<PagedList<ProductResponseDto>>(false, $"Internal Server Error: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ProductResponseDto>> UpdateAsync(UpdateProductRequestDto request) {
			try {
				var productFromDB = await _unitOfWork.Product.GetAsync(x => x.Id == request.Id);

				if (productFromDB is null) {
					return new ApiResponse<ProductResponseDto>(false, $"Product with the Id: {request.Id} does not exist");
				}

				await _unitOfWork.Product.BeginTransactionAsync();

				productFromDB.Name = request.Name!;
				productFromDB.Description = request.Description;
				productFromDB.Price = request.Price;
				productFromDB.ProductStatus = request.ProductStatus;
				productFromDB.Sold = request.Sold;
				productFromDB.BrandId = request.BrandId;
				productFromDB.Stock = request.Stock;
				productFromDB.Thumnail = request.Thumnail;
				productFromDB.UpdatedAt = DateTime.Now;
				productFromDB.UpdatedBy = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

				await _unitOfWork.Product.UpdateAsync(productFromDB);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.Product.EndTransactionAsync();

				var result = _mapper.Map<ProductResponseDto>(productFromDB);

				return new ApiResponse<ProductResponseDto>(result, true, "Updated successfully");
			} catch (Exception ex) {
				await _unitOfWork.Product.RollBackTransactionAsync();
				_logger.LogExceptions(ex);
				return new ApiResponse<ProductResponseDto>(false, $"Internal Server Error: {ex.Message}");
			}
		}
	}
}
