using System.Collections.Concurrent;
using System.Security.Cryptography;

using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Data_Access_layer.Repositories;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EMS_Project.Logical_Layer.Services
{
    public class Authservice : IAuthService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly EMSDbContext _context;
        private readonly IJWTService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly TimeSpan _tokenExpiry = TimeSpan.FromMinutes(5);

        private static ConcurrentDictionary<string, (string Token, DateTime Expiry)> _tokens = new();
        public Authservice(IAdminRepository adminRepository, EMSDbContext dbContext, IJWTService jWTService, IPasswordService passwordService, IEmailService emailService)
        {
            _adminRepository = adminRepository;
            _context = dbContext;
            _jwtService = jWTService;
            _passwordService = passwordService;
            _emailService = emailService;
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

        public async Task<IActionResult> ResetTokenGeneration(string email, string role)
        {
            try
            {

                if (role.ToLower().Trim() == "employee")
                {
                    var employee = _context.Employees.FirstOrDefault(e => e.Email == email);
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

                    var tokenBytes = new byte[32];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(tokenBytes);
                    }

                    var stringtokenBytes = Convert.ToBase64String(tokenBytes);
                    var expiry = DateTime.UtcNow.Add(_tokenExpiry);
                    _tokens[email] = (stringtokenBytes, expiry);

                    //var resetLink = $"https://localhost:7213/Enter-New-Password?Email={employee.Email}&Token={stringtokenBytes}";

                    //await _emailService.SendEmailAsync(employee.Email, "Password Reset",
                    //    $"Click <a href='{resetLink}'>here</a> to reset your password. This link expires in 5 minutes.");

                    return new ObjectResult(new
                    {
                        Success = true,
                        Token = stringtokenBytes,
                        Message = "Reset link Sent"
                    })
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else if (role.ToLower().Trim() == "admin")
                {
                    var admin = _context.Admins.FirstOrDefault(e => e.Email == email);
                    if (admin == null)
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

                    var tokenBytes = new byte[32];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(tokenBytes);
                    }

                    var stringtokenBytes = Convert.ToBase64String(tokenBytes);
                    var expiry = DateTime.UtcNow.Add(_tokenExpiry);
                    _tokens[email] = (stringtokenBytes, expiry);
                    //var resetLink = $"https://localhost:7213/Enter-New-Password?Email={admin.Email}&Token={stringtokenBytes}";
                    //await _emailService.SendEmailAsync(admin.Email, "Password Reset",
                    //    $"Click <a href='{resetLink}'>here</a> to reset your password. This link expires in 5 minutes.");

                    return new ObjectResult(new
                    {
                        Success = true,
                        Token = stringtokenBytes,
                        Message = "Reset Token Sent"
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

                        Message = "Role doesnot exist"
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

                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };

            }
        }

        public async Task<IActionResult> TokenValidation(ResetPasswordDTO resetPasswordRequest)
        {
            try
            {
                if (resetPasswordRequest.Role.ToLower().Trim() == "employee")
                {
                    if (_tokens.TryGetValue(resetPasswordRequest.Email, out var tokeninfo))
                    {
                        if (tokeninfo.Token == resetPasswordRequest.Token && tokeninfo.Expiry > DateTime.UtcNow)
                        {
                            var empdata = await _context.Employees.FirstOrDefaultAsync(e => e.Email == resetPasswordRequest.Email);

                            string newhashpassword = _passwordService.HashPassword(resetPasswordRequest.NewPassword);

                            empdata.PasswordHash = newhashpassword;

                            await _context.SaveChangesAsync();

                            //make token invalid
                            _tokens[resetPasswordRequest.Email] = (null, DateTime.MinValue);

                            return new ObjectResult(new
                            {
                                Success = true,
                                Message = "Password Reset Successful"
                            })
                            {
                                StatusCode = StatusCodes.Status200OK
                            };
                        }
                        else
                        {
                            _tokens.TryRemove(resetPasswordRequest.Email, out _);
                            return new ObjectResult(new
                            {
                                Success = false,
                                Message = "Token expired or wrong token sent.Try Generating new token."
                            })
                            {
                                StatusCode = StatusCodes.Status404NotFound
                            };
                        }

                    }
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "wrong email entered."
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                else if (resetPasswordRequest.Role.ToLower().Trim() == "admin")
                {
                    if (_tokens.TryGetValue(resetPasswordRequest.Email, out var tokeninfo))
                    {
                        if (tokeninfo.Token == resetPasswordRequest.Token && tokeninfo.Expiry > DateTime.UtcNow)
                        {
                            var admindata = await _context.Admins.FirstOrDefaultAsync(e => e.Email == resetPasswordRequest.Email);

                            string newhashpassword = _passwordService.HashPassword(resetPasswordRequest.NewPassword);

                            admindata.PasswordHash = newhashpassword;

                            await _context.SaveChangesAsync();

                            //make token invalid
                            _tokens[resetPasswordRequest.Email] = (null, DateTime.MinValue);

                            return new ObjectResult(new
                            {
                                Success = true,
                                Message = "Password Reset Successful"
                            })
                            {
                                StatusCode = StatusCodes.Status200OK
                            };
                        }
                        else
                        {
                            _tokens.TryRemove(resetPasswordRequest.Email, out _);
                            return new ObjectResult(new
                            {
                                Success = false,
                                Message = "Token expired or wrong token sent.Try Generating new token."
                            })
                            {
                                StatusCode = StatusCodes.Status404NotFound
                            };
                        }

                    }
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "wrong email entered."
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };

                }
                else
                {
                    return new ObjectResult(new
                    {
                        Success = false,
                        Message = "wrong role entered."
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
            }
            catch (Exception)
            {

                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Something went wrong."
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

    }
}
