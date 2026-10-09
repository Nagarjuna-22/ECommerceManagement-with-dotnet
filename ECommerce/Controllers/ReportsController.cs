using ECommerceOrderManagement.DTOs.Common;
using ECommerceOrderManagement.DTOs.Reports;
using ECommerceOrderManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.Controllers
{
    [ApiController]
    [Route("api/reports/[Action]")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var report =
                await _service.GetDashboardAsync();

            var response =
                new ApiResponse<DashboardReportDto>
                {
                    Status = true,
                    Message = "Dashboard report retrieved successfully.",
                    Data = report
                };

            return Ok(response);
        }
    }
}