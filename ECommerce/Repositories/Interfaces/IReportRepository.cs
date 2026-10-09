using ECommerceOrderManagement.DTOs.Reports;

namespace ECommerceOrderManagement.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<int> GetTotalProductsAsync();

        Task<int> GetTotalCustomersAsync();

        Task<int> GetTotalOrdersAsync();

        Task<decimal> GetTotalRevenueAsync();

        Task<TopSellingProductDto?> GetTopSellingProductAsync();

        Task<decimal> GetAverageOrderValueAsync();

        Task<List<CategoryProductCountDto>> GetCategoryWiseProductCountAsync();
    }
}