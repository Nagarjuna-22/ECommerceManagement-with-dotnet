using ECommerceOrderManagement.DTOs.Orders;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateAsync(
            CreateOrderDto dto);

        Task<List<OrderResponseDto>> GetAllAsync();

        Task<OrderResponseDto?> GetByIdAsync(int id);

        Task<List<OrderResponseDto>> GetByCustomerIdAsync(
            int customerId);

        Task<OrderResponseDto?> CancelAsync(int id);
    }
}