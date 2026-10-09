using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Categories
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}