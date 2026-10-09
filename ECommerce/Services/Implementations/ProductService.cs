using ECommerceOrderManagement.DTOs.Products;
using ECommerceOrderManagement.Models;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Interfaces;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.CategoryName ?? string.Empty
            }).ToList();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? string.Empty
            };
        }

        public async Task<ProductResponseDto> CreateAsync(
            CreateProductDto dto)
        {
            var category = await _categoryRepository
                .GetByIdAsync(dto.CategoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    "Category not found.");

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };

            await _productRepository.AddAsync(product);

            product.Category = category;

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<ProductResponseDto?> UpdateAsync(
            int id,
            UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            var category = await _categoryRepository
                .GetByIdAsync(dto.CategoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    "Category not found.");

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _productRepository.UpdateAsync(product);

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<ProductResponseDto?> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            if (product.OrderItems.Any())
            {
                throw new InvalidOperationException(
                    "Product cannot be deleted because it is associated with orders.");
            }

            var deletedProduct = new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName =
                    product.Category?.CategoryName ?? string.Empty
            };

            await _productRepository.DeleteAsync(product);

            return deletedProduct;
        }
    }
}