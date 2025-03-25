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
            var timesheets = await _context.Timesheets.Select(ts => new 
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

        public async Task<IActionResult> GetTimeSheet(int empid)
        {
            
            var timesheetdata =await _context.Employees.Where(e => e.EmployeeId == empid).Include(e => e.Timesheets).Select(
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
            var employee = await _context.Employees
        .Where(e => e.EmployeeId == empid)
        .Include(e => e.Timesheets)
        .FirstOrDefaultAsync();



            if (employee == null)
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
            var timesheetdata = employee.Timesheets.FirstOrDefault(ts => ts.Date == timesheetupdate.Date);

            if (timesheetdata == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Timesheet not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            timesheetdata.StartTime = timesheetupdate.StartTime;
            timesheetdata.EndTime = timesheetupdate.EndTime;
            timesheetdata.TotalHours = timesheetupdate.TotalHours;
            if (timesheetupdate.Description != null)
            {
                timesheetdata.Description = timesheetupdate.Description;
            }      

            
            await _context.SaveChangesAsync();

            return new ObjectResult( new
            {
                Success = true,
                Message = "Timesheet Updated Successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
