using ECommerceOrderManagement.Models;

namespace ECommerceOrderManagement.DTOs.Orders
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}