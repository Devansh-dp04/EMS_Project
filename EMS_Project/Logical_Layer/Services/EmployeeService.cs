using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Logical_Layer.Services
{
    public class EmployeeService : IEmployeeServices
    {
        private readonly EMSDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public EmployeeService(EMSDbContext context, IPasswordService passwordService, IMapper mapper)
        {
            _context = context;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetEmployee()
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
                }).ToListAsync();

                return new ObjectResult(new
                {
                    Success = true,
                    Data = empdata
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {

                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Error in fetching data"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    
        public async Task<IActionResult> GetEmployeeById(int id)
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
                if (empdata != null)
                {
                    return new ObjectResult(new
                    {
                        Success = true,
                        Data = empdata
                    })
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "Employee id not found"
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

            }
            catch (Exception ex)
            {
                
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Error in fetching data"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<IActionResult> AddEmployee(AddEmployeeDTO addEmployee)
        {
            if (await _context.Employees.AnyAsync(u => u.Email == addEmployee.Email))
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Email already registered"
                })
                {
                    StatusCode = StatusCodes.Status409Conflict
                };

            }
            var departmentExists = await _context.Departments.FindAsync(addEmployee.DepartmentId);
            if(departmentExists == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Department does not exist"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            string passwordhash = _passwordService.HashPassword(addEmployee.Password);
            var employee = _mapper.Map<Employee>(addEmployee);

            employee.PasswordHash = passwordhash;
            var department = _mapper.Map<Department>(addEmployee);

            //creating relationship
            employee.DepartmentId = department.DepartmentId;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new ObjectResult(new
            {
                Success = true,
                Message = "Employee Added Successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<IActionResult> DeleteEmployee(string email)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
                if (employee == null || employee.IsActive == false)
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "Employee not found"
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                employee.IsActive = false;

                await _context.SaveChangesAsync();
                return new ObjectResult(new
                {
                    Success = true,
                    Message = "Employee Deleted Successfully"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {

                return new ObjectResult(new
                {
                    Success = false,
                    Message = "An error occured while deleting"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<IActionResult> UpdateEmployeeByAdmin(JsonPatchDocument<UpdateEmployeeByAdminDTO> patchDoc, int empid)
        {

            try
            {
                if (patchDoc == null)
                {
                    throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null.");
                }

                var employee = await _context.Employees.FindAsync(empid);

                if (employee == null)
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "Employee not found"
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var employeeDTO = _mapper.Map<UpdateEmployeeByAdminDTO>(employee);

                patchDoc.ApplyTo(employeeDTO);

                _mapper.Map(employeeDTO, employee);

                employee.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return new ObjectResult(new
                {
                    Success = true,
                    Message = "Employee Detail Updated Successfully"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {

                return new ObjectResult(new
                {
                    Success = false,
                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<IActionResult> GetEmployeeByEmail(string email)
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
            }).FirstOrDefaultAsync(emp => emp.Email == email);
            if (empdata == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    message = "Email not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            return new ObjectResult(new
            {
                Success = true,
                Data = empdata
            })
            {
                StatusCode = StatusCodes.Status200OK
            };

        }

        public async Task<IActionResult> LogWorkingHours(LogWorkingHoursDTO logWorkingHours)
        {
            var empdata = await _context.Employees.FirstOrDefaultAsync(emp => emp.EmployeeId == logWorkingHours.EmployeeId);
            if (empdata == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    message = "Invalid EmployeeID"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var timesheet = _mapper.Map<Timesheet>(logWorkingHours);
            timesheet.EmployeeId = empdata.EmployeeId;

            _context.Timesheets.Add(timesheet);
            await _context.SaveChangesAsync();

            return new ObjectResult(new
            {
                Success = true,
                Message = "Working hours logged successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<IActionResult> UpdateEmployeeByEmployee(JsonPatchDocument<UpdateEmployeeByEmployeeDTO> patchDoc, int empid)
        {
            try
            {
                if (patchDoc == null)
                {
                    throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null.");
                }

                var employee = await _context.Employees.FindAsync(empid);

                if (employee == null)
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "Employee not found"
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                var employeeDTO = _mapper.Map<UpdateEmployeeByEmployeeDTO>(employee);

                patchDoc.ApplyTo(employeeDTO);

                _mapper.Map(employeeDTO, employee);

                employee.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return new ObjectResult(new
                {
                    Success = true,
                    Message = "Your detail has been successfully updated"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {

                return new ObjectResult(new
                {
                    Success = false,
                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
 }

