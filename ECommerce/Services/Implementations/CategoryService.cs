using ECommerceOrderManagement.DTOs.Categories;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Interfaces;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(category => new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            }).ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<CategoryResponseDto> CreateAsync(
            CreateCategoryDto dto)
        {
            var category = new Models.Category
            {
                CategoryName = dto.CategoryName
            };

            var createdCategory =
                await _repository.AddAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = createdCategory.CategoryId,
                CategoryName = createdCategory.CategoryName
            };
        }

        public async Task<CategoryResponseDto?> UpdateAsync(
            int id,
            UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            category.CategoryName = dto.CategoryName;

            await _repository.UpdateAsync(category);

            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<CategoryResponseDto?> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            if (category.Products.Any())
            {
                throw new InvalidOperationException(
                    "Category cannot be deleted because products exist under this category.");
            }

            var deletedCategory = new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };

            await _repository.DeleteAsync(category);

            return deletedCategory;
        }
    }
}