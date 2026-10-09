using ECommerceOrderManagement.Models;

namespace ECommerceOrderManagement.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<List<Order>> GetByCustomerIdAsync(int customerId);

        Task<Order> AddAsync(Order order);

        Task DeleteAsync(Order order);

        Task SaveChangesAsync();
    }
}