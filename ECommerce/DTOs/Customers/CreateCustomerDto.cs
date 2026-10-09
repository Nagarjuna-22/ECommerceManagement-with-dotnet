using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Customers
{
    public class CreateCustomerDto
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}