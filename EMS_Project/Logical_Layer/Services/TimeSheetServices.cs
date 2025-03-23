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
        EMSDbContext _context;
        public TimeSheetServices(EMSDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task< byte[] > ExportToExcelAsync()
        {
            var timesheets = await _context.Timesheets.Select(ts => new TimeSheetExportDTO
            {
                Date = ts.Date,
                StartTime = ts.StartTime,
                EndTime = ts.EndTime,
                TotalHours = ts.TotalHours,
                Description = ts.Description,
                CreatedAt = ts.CreatedAt
            }).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Timesheet");

                // Create header row
                worksheet.Cell(1, 1).Value = "Date";
                worksheet.Cell(1, 2).Value = "Start Time";
                worksheet.Cell(1, 3).Value = "End Time";
                worksheet.Cell(1, 4).Value = "Total Hours";
                worksheet.Cell(1, 5).Value = "Description";
                worksheet.Cell(1, 6).Value = "Created At";

                // Set header style
                var headerRange = worksheet.Range("A1:F1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Add data rows
                int row = 2;
                foreach (var timesheet in timesheets)
                {
                    worksheet.Cell(row, 1).Value = timesheet.Date;
                    worksheet.Cell(row, 2).Value = timesheet.StartTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 3).Value = timesheet.EndTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 4).Value = timesheet.TotalHours;
                    worksheet.Cell(row, 5).Value = timesheet.Description;
                    worksheet.Cell(row, 6).Value = timesheet.CreatedAt;
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
    }
}
