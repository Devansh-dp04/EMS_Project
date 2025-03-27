using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;

namespace EMS_Project.Data_Access_layer.Repositories
{
    
    public class TimeSheetRepository : ITimesheetRepository
    {
        private readonly EMSDbContext _context;
        public TimeSheetRepository(EMSDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<TimeSheetDTO>> GetTimeSheet()
        {
           var timesheetdata = await _context.Timesheets.Select(ts => new TimeSheetDTO
            {
               EmployeeId = ts.EmployeeId,
               Date = ts.Date,
                StartTime = ts.StartTime,
                EndTime = ts.EndTime,
                TotalHours = ts.TotalHours,
                Description = ts.Description,
                CreatedAt = ts.CreatedAt
            }).ToListAsync();

            return timesheetdata;
        }

        public async Task<object> GetTimeSheetById(int id)
        {
           var timesheetdata = await _context.Employees.Where(e => e.EmployeeId == id).Include(e => e.Timesheets).Select(
               emp => new
               {
                   emp.FirstName,
                   emp.LastName,
                   timesheet = emp.Timesheets.Select(t => new
                   {

                       t.Date,
                       t.CreatedAt,
                       t.StartTime,
                       t.EndTime,
                       t.Description

                   })

               }).ToListAsync();
            return timesheetdata;
        }

        
        public async Task<Timesheet?> UpdateTimeSheet(TimeSheetUpdateDTO timeSheet, int empid)
        {
            var employee = await _context.Employees
        .Where(e => e.EmployeeId == empid)
        .Include(e => e.Timesheets)
        .FirstOrDefaultAsync();
            if (employee == null)
            {
                return null;
            }
            var timesheetdata = employee.Timesheets.FirstOrDefault(ts => ts.Date == timeSheet.Date);
            if (timesheetdata == null) {
                return null;
            }
            timesheetdata.StartTime = timeSheet.StartTime;
            timesheetdata.EndTime = timeSheet.EndTime;
            timesheetdata.TotalHours = timeSheet.TotalHours;
            if (!timeSheet.Description.IsNullOrEmpty())
            {
                timesheetdata.Description = timeSheet.Description;
            }
            await _context.SaveChangesAsync();
            return timesheetdata;
        }

        public async Task<List<WeeklyReportDTO>> GetWeeklyReportAsync(int employeeId, DateTime startDate)
        {
            var startOfWeek = startDate.Date.AddDays(-(int)startDate.DayOfWeek + 1);  // Monday
            var endOfWeek = startOfWeek.AddDays(6);  // Sunday

            var report = await _context.Timesheets
                .Where(ts => ts.EmployeeId == employeeId &&
                             ts.Date >= startOfWeek && ts.Date <= endOfWeek)
                .GroupBy(ts => new
                {
                    Year = ts.Date.Year,
                    Week = EF.Functions.DateDiffWeek(new DateTime(1900, 1, 1), ts.Date)
                })
                .Select(g => new WeeklyReportDTO
                {
                    EmployeeId = employeeId,
                    WeekStart = startOfWeek,
                    WeekEnd = endOfWeek,
                    TotalHours = g.Sum(ts => ts.TotalHours),
                    TimesheetEntries = g.Select(ts => new TimeSheetDTO
                    {
                        Date = ts.Date,
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime,
                        TotalHours = ts.TotalHours,
                        Description = ts.Description
                    }).ToList()
                })
                .ToListAsync();

            return report;
        }

        public async Task<List<MonthlyReportDTO>> GetMonthlyReportAsync(int employeeId, int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var report = await _context.Timesheets
                .Where(ts => ts.EmployeeId == employeeId &&
                             ts.Date >= startOfMonth && ts.Date <= endOfMonth)
                .GroupBy(ts => new
                {
                    Year = ts.Date.Year,
                    Month = ts.Date.Month
                })
                .Select(g => new MonthlyReportDTO
                {
                    EmployeeId = employeeId,
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalHours = g.Sum(ts => ts.TotalHours),
                    TimesheetEntries = g.Select(ts => new TimeSheetDTO
                    {
                        Date = ts.Date,
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime,
                        TotalHours = ts.TotalHours,
                        Description = ts.Description
                    }).ToList()
                })
                .ToListAsync();

            return report;
        }
    }
}
