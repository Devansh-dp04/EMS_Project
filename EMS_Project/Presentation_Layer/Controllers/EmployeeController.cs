using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Presentation_Layer.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmployeeServices _employeeServices;
        private readonly ITimeSheetService _timeSheetService;

        public EmployeeController(IAuthService authService, IEmployeeServices employeeServices, ITimeSheetService timeSheetService)
        {
            _authService = authService;
            _employeeServices = employeeServices;
            _timeSheetService = timeSheetService;
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
