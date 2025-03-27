using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface ITimesheetRepository
    {
        public Task<List<TimeSheetDTO>> GetTimeSheet();

        public Task<object> GetTimeSheetById(int id);

        public  Task<Timesheet?> UpdateTimeSheet(TimeSheetUpdateDTO timeSheet, int empid);

        public Task<List<WeeklyReportDTO>> GetWeeklyReportAsync(int employeeId, DateTime startDate);
        public Task<List<MonthlyReportDTO>> GetMonthlyReportAsync(int employeeId, int year, int month);

    }
}
