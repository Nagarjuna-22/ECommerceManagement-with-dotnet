using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Products
{
    public class CreateProductDto
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}