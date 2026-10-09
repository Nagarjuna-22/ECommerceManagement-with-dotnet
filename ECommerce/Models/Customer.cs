using ECommerceOrderManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}