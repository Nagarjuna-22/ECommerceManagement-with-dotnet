using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.DTOs.Orders;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/orders/[Action]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderDto dto)
        {
            var order = await _service.CreateAsync(dto);

            var response = new ApiResponse<OrderResponseDto>
            {
                Status = true,
                Message = "Order created successfully.",
                Data = order
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.OrderId },
                response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();

            var response = new ApiResponse<List<OrderResponseDto>>
            {
                Status = true,
                Message = "Orders retrieved successfully.",
                Data = orders
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);

            if (order == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message = "Order not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<OrderResponseDto>
            {
                Status = true,
                Message = "Order retrieved successfully.",
                Data = order
            };

            return Ok(response);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerOrderHistory(
            int customerId)
        {
            var orders =
                await _service.GetByCustomerIdAsync(customerId);

            var response =
                new ApiResponse<List<OrderResponseDto>>
                {
                    Status = true,
                    Message = "Customer order history retrieved successfully.",
                    Data = orders
                };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);

            if (result == null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    Status = false,
                    Message = "Order not found.",
                    Data = null
                };

                return NotFound(errorResponse);
            }

            var response = new ApiResponse<OrderResponseDto>
            {
                Status = true,
                Message = "Order cancelled successfully.",
                Data = result
            };

            return Ok(response);
        }
    }
}