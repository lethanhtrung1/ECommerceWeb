using ApplicationLayer.DTOs.Pagination;
using ApplicationLayer.DTOs.Request.Product;
using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ApplicationLayer.Common.Constant;

namespace WebAPI.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase {
		private readonly IProductService _productService;

		public ProductController(IProductService productService) {
			_productService = productService;
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("product/{Id:Guid}")]
		public async Task<IActionResult> GetProductById(Guid productId) {
			if (productId == Guid.Empty) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _productService.GetProductAsync(productId));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpGet("product")]
		public async Task<IActionResult> GetProducts([FromQuery] PagingRequest request) {
			if (request == null) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _productService.GetProductsAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("product")]
		public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestDto request) {
			if (request == null) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _productService.AddAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPut("product")]
		public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto request) {
			if (request == null) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _productService.UpdateAsync(request));
		}

		[Authorize(Roles = Role.Admin)]
		[HttpDelete("product")]
		public async Task<IActionResult> DeleteProduct(Guid productId) {
			if (productId == Guid.Empty) {
				return BadRequest("Invalid payload");
			}
			return Ok(await _productService.DeleteAsync(productId));
		}
	}
}
