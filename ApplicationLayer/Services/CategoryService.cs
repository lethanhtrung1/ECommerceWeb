using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Category;
using ApplicationLayer.DTOs.Response;
using ApplicationLayer.DTOs.Response.Category;
using ApplicationLayer.Interfaces;
using ApplicationLayer.Logging;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Repositories;

namespace ApplicationLayer.Services {
	public class CategoryService : ICategoryService {
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogException _logger;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork, ILogException logger, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<GeneralResponse> AddCategory(CreateCategoryRequestDto request) {
			try {
				if (request is null) {
					return new GeneralResponse(false, "Invalid payload");
				}

				var checkCategory = await _unitOfWork.Category.GetAsync(x => x.CategoryName == request.CategoryName);
				if (checkCategory is not null) {
					return new GeneralResponse(false, "Category already exits");
				}

				var newCategoryToDb = _mapper.Map<Category>(request);
				await _unitOfWork.Category.BeginTransactionAsync();
				await _unitOfWork.Category.AddAsync(newCategoryToDb);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.Category.EndTransactionAsync();

				return new GeneralResponse(true, "Create new Category is success");
			} catch (Exception ex) {
				await _unitOfWork.Category.RollBackTransactionAsync();
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> DeleteCategories(IEnumerable<Guid> categoryIds) {
			try {
				var categories = await _unitOfWork.Category.GetListAsync(x => categoryIds.Contains(x.Id));

				if (categories is null || categories.Count() == 0) {
					return new GeneralResponse(false, "No record available");
				}

				await _unitOfWork.Category.RemoveRangeAsync(categories);
				await _unitOfWork.SaveChangesAsync();

				return new GeneralResponse(true, "Deleted successfully");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> DeleteCategory(Guid categoryId) {
			try {
				var category = await _unitOfWork.Category.GetAsync(x => x.Id == categoryId);

				if (category is null) {
					return new GeneralResponse(false, $"Category with Id: {categoryId} does not exits");
				}

				await _unitOfWork.Category.RemoveAsync(category);
				await _unitOfWork.SaveChangesAsync();

				return new GeneralResponse(true, "Deleted successfully");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}

		public async Task<ApiResponse<PagedList<CategoryResponseDto>>> GetCategories(PagingRequest request) {
			try {
				var categories = await _unitOfWork.Category.GetListAsync(x => x.Published == true);
				var categoriesPageList = categories.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
				int totalRecord = categories.Count();

				if (categoriesPageList is null || categoriesPageList.Count() == 0) {
					return new ApiResponse<PagedList<CategoryResponseDto>>(false, "No record available");
				}

				var result = _mapper.Map<List<CategoryResponseDto>>(categoriesPageList);

				return new ApiResponse<PagedList<CategoryResponseDto>>(
					new PagedList<CategoryResponseDto>(
							result, request.PageNumber, request.PageSize, totalRecord
						), true, ""
					);
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new ApiResponse<PagedList<CategoryResponseDto>>(false, ex.Message);
			}
		}

		public async Task<ApiResponse<CategoryResponseDto>> GetCategoryById(Guid categoryId) {
			try {
				if (categoryId == Guid.Empty) {
					return new ApiResponse<CategoryResponseDto>(false, "Invalid payload");
				}

				var category = await _unitOfWork.Category.GetAsync(x => x.Id == categoryId);

				if (category is null) {
					return new ApiResponse<CategoryResponseDto>(false, $"Category with Id: {categoryId} does not exits");
				}

				var result = _mapper.Map<CategoryResponseDto>(category);
				return new ApiResponse<CategoryResponseDto>(result, true, "");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new ApiResponse<CategoryResponseDto>(false, ex.Message);
			}
		}

		public async Task<GeneralResponse> UpdateCategory(UpdateCategoryRequestDto request) {
			try {
				if (request is null) {
					return new GeneralResponse(false, "Invalid payload");
				}

				var categoryFromDb = await _unitOfWork.Category.GetAsync(x => x.Id == request.Id);

				if (categoryFromDb is not null) {
					categoryFromDb.CategoryName = request.CategoryName!;
					categoryFromDb.Description = request.Description;
					categoryFromDb.Published = request.Published;
					categoryFromDb.UpdatedAt = DateTime.Now;
					await _unitOfWork.Category.UpdateAsync(categoryFromDb);
				}

				await _unitOfWork.SaveChangesAsync();

				return new GeneralResponse(true, "Category updated successfully");
			} catch (Exception ex) {
				_logger.LogExceptions(ex);
				return new GeneralResponse(false, ex.Message);
			}
		}
	}
}
