using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Logical_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Presentation_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITimeSheetService _timeSheetService;
        public LoginController(IAuthService authService, ITimeSheetService timeSheetService)
        {
            _authService = authService;
            _timeSheetService = timeSheetService;
        }
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin(AdminLoginReqeustDTO adminLoginReqeust)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _authService.LoginAdminAsync(adminLoginReqeust);
            }

        }

        [HttpPost("Employee-Login")]
        public async Task<IActionResult> EmployeeLogin(EmployeeLoginRequestDTO employeeLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _authService.LoginEmployeeAsync(employeeLoginRequest);
            }

        }
       

    }
}
