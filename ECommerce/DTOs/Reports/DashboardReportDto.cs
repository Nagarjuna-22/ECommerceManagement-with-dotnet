namespace ECommerceOrderManagement.DTOs.Reports
{
    public class DashboardReportDto
    {
        public int TotalProducts { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        public string? TopSellingProduct { get; set; }

        public int UnitsSold { get; set; }

        public decimal AverageOrderValue { get; set; }

        public List<CategoryProductCountDto>
            CategoryWiseProductCount
        { get; set; } = new();
    }
}