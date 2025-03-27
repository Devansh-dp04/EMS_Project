using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Logical_Layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Presentation_Layer.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ReportGenerateController : ControllerBase
    {
        private readonly ITimeSheetService _timeSheetService;
        public ReportGenerateController(ITimeSheetService timeSheetService) {
            _timeSheetService = timeSheetService;
        }

        [HttpGet("WeeklyReport")]
        public async Task<IActionResult> GetWeeklyReport(int employeeId, DateTime startDate)
        {
            var report = await _timeSheetService.GetWeeklyReportAsync(employeeId, startDate);

            if (report == null || !report.Any())
            {
                return NotFound(new { Message = "No weekly report found." });
            }

            return Ok(report);
        }

        [HttpGet("MonthlyReport")]
        public async Task<IActionResult> GetMonthlyReport(int employeeId, int year, int month)
        {
            var report = await _timeSheetService.GetMonthlyReportAsync(employeeId, year, month);

            if (report == null || !report.Any())
            {
                return NotFound(new { Message = "No monthly report found." });
            }

            return Ok(report);
        }
    }
}
