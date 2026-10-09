using System.ComponentModel.DataAnnotations;

namespace ECommerceOrderManagement.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemRequestDto> Items { get; set; } = new();
    }
}