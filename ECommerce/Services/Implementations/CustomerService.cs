using ECommerceOrderManagement.DTOs.Customers;
using ECommerceOrderManagement.Models;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Interfaces;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();

            return customers.Select(customer => new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            }).ToList();
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return null;

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }

        public async Task<CustomerResponseDto> CreateAsync(
            CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            var createdCustomer =
                await _repository.AddAsync(customer);

            return new CustomerResponseDto
            {
                CustomerId = createdCustomer.CustomerId,
                CustomerName = createdCustomer.CustomerName,
                Email = createdCustomer.Email,
                PhoneNumber = createdCustomer.PhoneNumber,
                Address = createdCustomer.Address
            };
        }

        public async Task<CustomerResponseDto?> UpdateAsync(
            int id,
            UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return null;

            customer.CustomerName = dto.CustomerName;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;

            await _repository.UpdateAsync(customer);

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }

        public async Task<CustomerResponseDto?> DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                return null;

            if (customer.Orders.Any())
            {
                throw new InvalidOperationException(
                    "Customer cannot be deleted because orders exist.");
            }

            var deletedCustomer = new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            await _repository.DeleteAsync(customer);

            return deletedCustomer;
        }
    }
}