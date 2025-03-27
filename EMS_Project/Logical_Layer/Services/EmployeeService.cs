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
using Org.BouncyCastle.Ocsp;

namespace EMS_Project.Logical_Layer.Services
{
    public class EmployeeService : IEmployeeServices
    {
        
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService( IPasswordService passwordService, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            
            _passwordService = passwordService;
            _mapper = mapper;
            _employeeRepository = employeeRepository;   
        }

        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                var empdata =await _employeeRepository.GetEmployeeAsync();

                return new ObjectResult(new
                {
                    Success = true,
                    Data = empdata
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception )
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

                var empdata =await _employeeRepository.GetEmployeeByIdAsync(id);
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
            try
            {
                var empexists = _employeeRepository.CheckIfEmployeeExistsByEmail(addEmployee.Email);

                if (empexists != null)
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
                var departmentExists = await _employeeRepository.CheckIfDepartmentExists(addEmployee.DepartmentId);
                if (departmentExists == null)
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

                await _employeeRepository.AddEmployee(employee);

                return new ObjectResult(new
                {
                    Success = true,
                    Message = "Employee Added Successfully"
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

        public async Task<IActionResult> DeleteEmployee(string email)
        {
            try
            {
                var employee = await _employeeRepository.CheckIfEmployeeExistsByEmail(email);
                if (employee == null )
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
                await _employeeRepository.DeleteEmployee(email);
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

                
                var employee = await _employeeRepository.CheckIfEmployeeExistsById(empid);

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

                var emailexists = await _employeeRepository.CheckIfEmployeeExistsByEmail(employee.Email);
                if (emailexists != null)
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

                await _employeeRepository.UpdateEmployee(employee);

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
            try
            {
                var empdata = await _employeeRepository.GetAdminEmployeeByEmailAsync(email);
                if (empdata == null)
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        message = "Employee not found"
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
            catch (Exception)
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

        public async Task<IActionResult> LogWorkingHours(LogWorkingHoursDTO logWorkingHours, int empid)
        {
            var empdata = await _employeeRepository.CheckIfEmployeeExistsById(empid);
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

            if (await _employeeRepository.logWorkingHours(timesheet) == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Working hours already logged"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            };      
            
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

                var employee = await _employeeRepository.CheckIfEmployeeExistsById(empid);

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
                await _employeeRepository.UpdateEmployee(employee);               

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

