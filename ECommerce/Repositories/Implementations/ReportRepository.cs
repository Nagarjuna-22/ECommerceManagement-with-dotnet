using ECommerceOrderManagement.Data;
using ECommerceOrderManagement.DTOs.Reports;
using ECommerceOrderManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_GetTotalProducts")
                .SingleAsync();
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_GetTotalCustomers")
                .SingleAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Database
                .SqlQueryRaw<int>(
                    "EXEC sp_GetTotalOrders")
                .SingleAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Database
                .SqlQueryRaw<decimal>(
                    "EXEC sp_GetTotalRevenue")
                .SingleAsync();
        }

        public async Task<TopSellingProductDto?>
            GetTopSellingProductAsync()
        {
            return await _context.Database
                .SqlQueryRaw<TopSellingProductDto>(
                    "EXEC sp_GetTopSellingProduct")
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetAverageOrderValueAsync()
        {
            return await _context.Database
                .SqlQueryRaw<decimal>(
                    "EXEC sp_GetAverageOrderValue")
                .SingleAsync();
        }

        public async Task<List<CategoryProductCountDto>>
            GetCategoryWiseProductCountAsync()
        {
            return await _context.Database
                .SqlQueryRaw<CategoryProductCountDto>(
                    "EXEC sp_GetCategoryWiseProductCount")
                .ToListAsync();
        }
    }
}