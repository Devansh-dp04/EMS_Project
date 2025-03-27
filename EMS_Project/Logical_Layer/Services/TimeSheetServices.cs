using ClosedXML.Excel;
using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Logical_Layer.Services
{
    public class TimeSheetServices : ITimeSheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        public TimeSheetServices(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task< byte[] > ExportToExcelAsync()
        {
            var timesheets = await _timesheetRepository.GetTimeSheet();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Timesheet");

                // Create header row
                worksheet.Cell(1, 1).Value = "Date";
                worksheet.Cell(1, 2).Value = "EmployeeID";
                worksheet.Cell(1, 3).Value = "Start Time";
                worksheet.Cell(1, 4).Value = "End Time";
                worksheet.Cell(1, 5).Value = "Total Hours";
                worksheet.Cell(1, 6).Value = "Description";
                worksheet.Cell(1, 7).Value = "Created At";
                

                // Set header style
                var headerRange = worksheet.Range("A1:G1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Add data rows
                int row = 2;

                foreach (var timesheet in timesheets)
                {
                    worksheet.Cell(row, 1).Value = timesheet.Date;
                    worksheet.Cell(row, 2).Value = timesheet.EmployeeId;
                    worksheet.Cell(row, 3).Value = timesheet.StartTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 4).Value = timesheet.EndTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 5).Value = timesheet.TotalHours;
                    worksheet.Cell(row, 6).Value = timesheet.Description;
                    worksheet.Cell(row, 7).Value = timesheet.CreatedAt;
                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Save to memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<IActionResult> GetTimeSheet(int empid)
        {
            
            var timesheetdata =await _timesheetRepository.GetTimeSheetById(empid);

            if (timesheetdata == null)
            {
                return new ObjectResult(new
                {
                    Success = false,

                    Message = "Empid not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            return new ObjectResult(new
            {
                Success = true,
                data = timesheetdata
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<IActionResult> UpdateTimeSheet(TimeSheetUpdateDTO timesheetupdate, int empid)
        {            

            var timesheetdata = await _timesheetRepository.UpdateTimeSheet(timesheetupdate, empid);

            if (timesheetdata == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Timesheet or employee ID not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }            

            return new ObjectResult( new
            {
                Success = true,
                Message = "Timesheet Updated Successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<List<WeeklyReportDTO>> GetWeeklyReportAsync(int employeeId, DateTime startDate)
        {
            return await _timesheetRepository.GetWeeklyReportAsync(employeeId, startDate);
        }
        public async Task<List<MonthlyReportDTO>> GetMonthlyReportAsync(int employeeId, int year, int month)
        {
            return await _timesheetRepository.GetMonthlyReportAsync(employeeId, year, month);
        }

    }
}
