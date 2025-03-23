using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface IAuthService
    {
        public Task<IActionResult> LoginAdminAsync(AdminLoginReqeustDTO adminLoginReqeustDTO);

        public Task<IActionResult> LoginEmployeeAsync(EmployeeLoginRequestDTO employeeLoginRequestDTO);

    }
}
