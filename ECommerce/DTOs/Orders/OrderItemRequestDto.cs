using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Orders
{
    public class OrderItemRequestDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}