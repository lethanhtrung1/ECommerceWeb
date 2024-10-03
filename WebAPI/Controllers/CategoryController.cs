using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Category;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApplicationLayer.Common.Constant;

namespace WebAPI.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase {
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService) {
			_categoryService = categoryService;
		}

		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetCategoryById(Guid categoryId) {
			if (categoryId == Guid.Empty) {
				return BadRequest("Invalid request");
			}
			return Ok(await _categoryService.GetCategoryById(categoryId));
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories([FromQuery] PagingRequest request) {
			if (request == null) {
				return BadRequest("Invalid request");
			}
			return Ok(await _categoryService.GetCategories(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost]
		public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequestDto request) {
			if (request == null) {
				return BadRequest("Invalid request");
			}
			return Ok(await _categoryService.AddCategory(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPut]
		public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestDto request) {
			if (request == null) {
				return BadRequest("Invalid request");
			}
			return Ok(await _categoryService.UpdateCategory(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpDelete]
		public async Task<IActionResult> DeleteCategory(Guid categoryId) {
			if (categoryId == Guid.Empty) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _categoryService.DeleteCategory(categoryId));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpDelete("remove-list")]
		public async Task<IActionResult> DeleteCategories(IEnumerable<Guid> categoryIds) {
			if (categoryIds is null || categoryIds.Count() == 0) {
				return BadRequest("Invalid request");
			}
			return Ok(await _categoryService.DeleteCategories(categoryIds));
		}
	}
}
