using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.DTOs.Customers;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/customers/[Action]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateCustomerDto dto)
        {
            var customer = await _service.CreateAsync(dto);

            var response =
                new ApiResponse<CustomerResponseDto>
                {
                    Status = true,
                    Message = "Customer created successfully.",
                    Data = customer
                };

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();

            var response =
                new ApiResponse<IEnumerable<CustomerResponseDto>>
                {
                    Status = true,
                    Message = "Customers retrieved successfully.",
                    Data = customers
                };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);

            if (customer == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Customer with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response =
                new ApiResponse<CustomerResponseDto>
                {
                    Status = true,
                    Message = "Customer retrieved successfully.",
                    Data = customer
                };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCustomerDto dto)
        {
            var customer = await _service.UpdateAsync(id, dto);

            if (customer == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Customer with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response =
                new ApiResponse<CustomerResponseDto>
                {
                    Status = true,
                    Message = "Customer updated successfully.",
                    Data = customer
                };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _service.DeleteAsync(id);

            if (customer == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message =
                        $"Customer with ID {id} was not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response =
                new ApiResponse<CustomerResponseDto>
                {
                    Status = true,
                    Message = "Customer deleted successfully.",
                    Data = customer
                };

            return Ok(response);
        }
    }
}