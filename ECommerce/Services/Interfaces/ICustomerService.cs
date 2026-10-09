using ECommerceOrderManagement.DTOs.Customers;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllAsync();

        Task<CustomerResponseDto?> GetByIdAsync(int id);

        Task<CustomerResponseDto> CreateAsync(
            CreateCustomerDto dto);

        Task<CustomerResponseDto?> UpdateAsync(
            int id,
            UpdateCustomerDto dto);

        Task<CustomerResponseDto?> DeleteAsync(int id);
    }
}