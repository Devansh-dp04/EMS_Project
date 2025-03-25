using DocumentFormat.OpenXml.Spreadsheet;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Presentation_Layer.Controllers
{
    [Authorize(Roles = "Employee")]
    [ApiController]
    [Route("api/employee/[controller]")]
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

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> EmployeeResetPassword(string email, string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _authService.ResetTokenGeneration(email,"employee");
            }

        }

        [HttpPost("Enter-New-Password")]
        public async Task<IActionResult> EnterNewPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _authService.TokenValidation(resetPasswordDTO);
            }
        }

        [HttpGet("Employee-Profile")]
        public async Task<IActionResult> GetEmployeeProfile(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _employeeServices.GetEmployeeByEmail(email);
            }
        }

        [HttpPost("Log-Working-Hours")]
        public async Task<IActionResult> LogWorkingHours([FromBody] LogWorkingHoursDTO logWorkingHoursDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _employeeServices.LogWorkingHours(logWorkingHoursDTO);
            }
        }

        [HttpPatch("Update-Employee")]
        public async Task<IActionResult> UpdateEmployeeByEmployee([FromBody] JsonPatchDocument<UpdateEmployeeByEmployeeDTO> patchDoc, [FromQuery] int empid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _employeeServices.UpdateEmployeeByEmployee(patchDoc, empid);
            }
        }

        [HttpGet("Get-TimeSheet-By-Id")]
        public async Task<IActionResult> GetTimeSheetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _timeSheetService.GetTimeSheet(id);
            }
        }

        [HttpPost("Update-TimeSheet")]
        public async Task<IActionResult> UpdateTimeSheet([FromBody]TimeSheetUpdateDTO timeSheetupdate,[FromQuery]int empid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _timeSheetService.UpdateTimeSheet(timeSheetupdate,empid);
            }
        }
    }
}

