using ECommerceOrderManagement.DTOs.Products;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();

        Task<ProductResponseDto?> GetByIdAsync(int id);

        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

        Task<ProductResponseDto?> UpdateAsync(
            int id,
            UpdateProductDto dto);

        Task<ProductResponseDto?> DeleteAsync(int id);
    }
}