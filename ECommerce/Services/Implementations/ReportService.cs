using ECommerceOrderManagement.DTOs.Reports;
using ECommerceOrderManagement.Repositories.Interfaces;
using ECommerceOrderManagement.Services.Interfaces;

namespace ECommerceOrderManagement.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<DashboardReportDto> GetDashboardAsync()
        {
            var totalProducts =
                await _reportRepository.GetTotalProductsAsync();

            var totalCustomers =
                await _reportRepository.GetTotalCustomersAsync();

            var totalOrders =
                await _reportRepository.GetTotalOrdersAsync();

            var totalRevenue =
                await _reportRepository.GetTotalRevenueAsync();

            var topSellingProduct =
                await _reportRepository.GetTopSellingProductAsync();

            var averageOrderValue =
                await _reportRepository.GetAverageOrderValueAsync();

            var categoryProductCount =
                await _reportRepository
                    .GetCategoryWiseProductCountAsync();

            return new DashboardReportDto
            {
                TotalProducts = totalProducts,

                TotalCustomers = totalCustomers,

                TotalOrders = totalOrders,

                TotalRevenue = totalRevenue,

                TopSellingProduct =
                    topSellingProduct?.ProductName
                    ?? "No sales yet",

                UnitsSold =
                    topSellingProduct?.UnitsSold
                    ?? 0,

                AverageOrderValue =
                    averageOrderValue,

                CategoryWiseProductCount =
                    categoryProductCount
            };
        }
    }
}