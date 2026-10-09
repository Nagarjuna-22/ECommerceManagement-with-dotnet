using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.DTOs.Categories;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/categories/[Action]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateCategoryDto dto)
        {
            var category = await _service.CreateAsync(dto);

            var response = new ApiResponse<CategoryResponseDto>
            {
                Status = true,
                Message = "Category created successfully.",
                Data = category
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();

            var response =
                new ApiResponse<IEnumerable<CategoryResponseDto>>
                {
                    Status = true,
                    Message = "Categories retrieved successfully.",
                    Data = categories
                };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);

            if (category == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Category with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<CategoryResponseDto>
            {
                Status = true,
                Message = "Category retrieved successfully.",
                Data = category
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCategoryDto dto)
        {
            var category = await _service.UpdateAsync(id, dto);

            if (category == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Category with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<CategoryResponseDto>
            {
                Status = true,
                Message = "Category updated successfully.",
                Data = category
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.DeleteAsync(id);

            if (category == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Category with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<CategoryResponseDto>
            {
                Status = true,
                Message = "Category deleted successfully.",
                Data = category
            };

            return Ok(response);
        }
    }
}