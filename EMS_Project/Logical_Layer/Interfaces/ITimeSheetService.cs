﻿using EMS_Project.Logical_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface ITimeSheetService
    {
        public Task<byte[]> ExportToExcelAsync();

        public Task<IActionResult> GetTimeSheet(int empid);

        public Task<IActionResult> UpdateTimeSheet(TimeSheetUpdateDTO timesheetupdate, int empid);
        public Task<List<WeeklyReportDTO>> GetWeeklyReportAsync(int employeeId, DateTime startDate);
        public Task<List<MonthlyReportDTO>> GetMonthlyReportAsync(int employeeId, int year, int month);
    }
}
