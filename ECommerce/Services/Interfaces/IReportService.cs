using ECommerceOrderManagement.DTOs.Reports;

namespace ECommerceOrderManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<DashboardReportDto> GetDashboardAsync();
    }
}