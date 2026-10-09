using ECommerceOrderManagement.DTOs.Categories;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();

        Task<CategoryResponseDto?> GetByIdAsync(int id);

        Task<CategoryResponseDto> CreateAsync(
            CreateCategoryDto dto);

        Task<CategoryResponseDto?> UpdateAsync(
            int id,
            UpdateCategoryDto dto);

        Task<CategoryResponseDto?> DeleteAsync(int id);
    }
}