using System.Security.Cryptography;
using System.Text;
using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Data_Access_layer.Repositories;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Services
{
    public class Authservice : IAuthService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly EMSDbContext _context;
        private readonly IJWTService _jwtService;
        private readonly IPasswordService _passwordService;
        public Authservice(IAdminRepository adminRepository, EMSDbContext dbContext, IJWTService jWTService, IPasswordService passwordService)
        {
            _adminRepository = adminRepository;
            _context = dbContext;
            _jwtService = jWTService;
            _passwordService = passwordService;
        }
        public async Task<IActionResult> LoginAdminAsync(AdminLoginReqeustDTO adminLoginReqeustDTO)
        {
            var admin = await _adminRepository.GetAdminByEmailAsync(adminLoginReqeustDTO.Email);
            if (admin == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Admin email not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (admin.PasswordHash != _passwordService.HashPassword(adminLoginReqeustDTO.Password))
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Invalid password"
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            string token = _jwtService.GenerateJWTToken(admin.Email, admin.PasswordHash, "Admin");
            return new ObjectResult(new
            {
                Success = true,
                Token = token,
                Message = "Login successful"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<IActionResult> LoginEmployeeAsync(EmployeeLoginRequestDTO employeeLoginRequestDTO)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Email == employeeLoginRequestDTO.Email);
            if (employee == null)
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Email not found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (employee.PasswordHash != _passwordService.HashPassword(employeeLoginRequestDTO.Password))
            {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Invalid password"
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            string token = _jwtService.GenerateJWTToken(employee.Email, employee.PasswordHash, "Employee");
            return new ObjectResult(new
            {
                Success = true,
                Token = token,
                Message = "Login successful"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        
    }
}
