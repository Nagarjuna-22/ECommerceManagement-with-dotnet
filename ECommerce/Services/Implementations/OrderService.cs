using ECommerceOrderManagement.Data;
using ECommerceOrderManagement.DTOs.Orders;
using ECommerceOrderManagement.Models;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<OrderResponseDto> CreateAsync(
            CreateOrderDto dto)
        {
            var customer =
                await _customerRepository.GetByIdAsync(dto.CustomerId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            if (dto.Items == null || dto.Items.Count == 0)
                throw new InvalidOperationException(
                    "Order must contain at least one item.");

            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending
                };

                decimal totalAmount = 0;

                foreach (var item in dto.Items)
                {
                    var product =
                        await _productRepository.GetByIdAsync(item.ProductId);

                    if (product == null)
                        throw new KeyNotFoundException(
                            $"Product {item.ProductId} not found.");

                    if (product.StockQuantity < item.Quantity)
                        throw new InvalidOperationException(
                            $"Insufficient stock for product {product.ProductName}.");

                    var subTotal =
                        product.Price * item.Quantity;

                    totalAmount += subTotal;

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    });

                    product.StockQuantity -= item.Quantity;
                }

                order.TotalAmount = totalAmount;

                await _orderRepository.AddAsync(order);

                await transaction.CommitAsync();

                return MapToDto(order);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return order == null ? null : MapToDto(order);
        }

        public async Task<List<OrderResponseDto>>
            GetByCustomerIdAsync(int customerId)
        {
            var orders =
                await _orderRepository.GetByCustomerIdAsync(customerId);

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderResponseDto?> CancelAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            if (order.Status == OrderStatus.Delivered)
                throw new InvalidOperationException(
                    "Delivered orders cannot be cancelled.");

            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException(
                    "Order is already cancelled.");

            foreach (var item in order.OrderItems)
            {
                if (item.Product != null)
                {
                    item.Product.StockQuantity += item.Quantity;
                }
            }

            order.Status = OrderStatus.Cancelled;

            await _orderRepository.SaveChangesAsync();

            return MapToDto(order);
        }

        private static OrderResponseDto MapToDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,

                Items = order.OrderItems.Select(item =>
                    new OrderItemResponseDto
                    {
                        ProductId = item.ProductId,
                        ProductName =
                            item.Product?.ProductName ?? string.Empty,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        SubTotal =
                            item.UnitPrice * item.Quantity
                    }).ToList()
            };
        }
    }
}