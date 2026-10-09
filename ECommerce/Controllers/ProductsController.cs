using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.DTOs.Products;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/products/[Action]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateProductDto dto)
        {
            var product = await _service.CreateAsync(dto);

            var response = new ApiResponse<ProductResponseDto>
            {
                Status = true,
                Message = "Product created successfully.",
                Data = product
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.ProductId },
                response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();

            var response =
                new ApiResponse<IEnumerable<ProductResponseDto>>
                {
                    Status = true,
                    Message = "Products retrieved successfully.",
                    Data = products
                };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Product with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<ProductResponseDto>
            {
                Status = true,
                Message = "Product retrieved successfully.",
                Data = product
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductDto dto)
        {
            var product = await _service.UpdateAsync(id, dto);

            if (product == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Product with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<ProductResponseDto>
            {
                Status = true,
                Message = "Product updated successfully.",
                Data = product
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.DeleteAsync(id);

            if (product == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Product with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<ProductResponseDto>
            {
                Status = true,
                Message = "Product deleted successfully.",
                Data = product
            };

            return Ok(response);
        }
    }
}