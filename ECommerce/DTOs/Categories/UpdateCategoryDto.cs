using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}