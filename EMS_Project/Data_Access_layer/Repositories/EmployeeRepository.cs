using DocumentFormat.OpenXml.Office2010.Excel;
using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Data_Access_layer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EMSDbContext _context;

        public EmployeeRepository(EMSDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetEmployeeAsync()
        {
            var empdata = await _context.Employees.Include(e => e.Timesheets).Select(empdata => new
            {
                empdata.EmployeeId,
                empdata.FirstName,
                empdata.LastName,
                empdata.Email,
                empdata.Phone,
                empdata.Department.DepartmentName,
                timesheets = empdata.Timesheets.Select(ts => new
                {
                    ts.CreatedAt,
                    ts.Date,
                    ts.TotalHours,
                    ts.StartTime,
                    ts.EndTime,

                })
            }).ToListAsync();

            return empdata;
        }
        public async Task<object?> GetAdminEmployeeByEmailAsync(string email)
        {
            try
            {
                var empdata = await _context.Employees.Include(e => e.Timesheets).Select(empdata => new
                {
                    empdata.EmployeeId,
                    empdata.FirstName,
                    empdata.LastName,
                    empdata.Email,
                    empdata.Phone,
                    empdata.Department.DepartmentName,
                    timesheets = empdata.Timesheets.Select(ts => new
                    {
                        ts.CreatedAt,
                        ts.Date,
                        ts.TotalHours,
                        ts.StartTime,
                        ts.EndTime,

                    })
                }).FirstOrDefaultAsync(e => e.Email == email);
                return empdata;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<object?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var empdata = await _context.Employees.Include(e => e.Timesheets).Select(empdata => new
                {
                    empdata.EmployeeId,
                    empdata.FirstName,
                    empdata.LastName,
                    empdata.Email,
                    empdata.Phone,
                    empdata.Department.DepartmentName,
                    timesheets = empdata.Timesheets.Select(ts => new
                    {
                        ts.CreatedAt,
                        ts.Date,
                        ts.TotalHours,
                        ts.StartTime,
                        ts.EndTime,

                    })
                }).FirstOrDefaultAsync(e => e.EmployeeId == id);
                return empdata;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Employee?> CheckIfEmployeeExistsByEmail(string email)
        {
            try
            {
                var empdata = await _context.Employees.FirstOrDefaultAsync(e =>e.IsActive && e.Email == email);
                return empdata;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Employee?> CheckIfEmployeeExistsById(int id)
        {
            try
            {
                var empdata = await _context.Employees.FirstOrDefaultAsync(e => e.IsActive && e.EmployeeId == id);
                return empdata;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department?> CheckIfDepartmentExists(int id)
        {
            try
            {
               var departmentdetail = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
                return departmentdetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object?> AddEmployee(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteEmployee(string email)
        {
            try
            {
                var empdata = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.IsActive);
                empdata.IsActive = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }                   
            
        }

        public async Task<object?> UpdateEmployee(Employee employee)
        {
            try
            {
                employee.UpdatedAt = DateTime.Now;
                 _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Timesheet?> logWorkingHours(Timesheet timesheet)
        {
            try
            {
                var empentryexists =await _context.Timesheets.FirstOrDefaultAsync(t => t.EmployeeId == timesheet.EmployeeId && (t.Date.Date) == timesheet.Date.Date);
                if (empentryexists != null)
                {
                    return null;
                }
                await _context.Timesheets.AddAsync(timesheet);
                await _context.SaveChangesAsync();
                return timesheet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
