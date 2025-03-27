using DocumentFormat.OpenXml.Spreadsheet;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Logical_Layer.Services;
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
        private readonly ILeaveService _leaveService;
        public EmployeeController(IAuthService authService, IEmployeeServices employeeServices, ITimeSheetService timeSheetService, ILeaveService leaveService)
        {
            _authService = authService;
            _employeeServices = employeeServices;
            _timeSheetService = timeSheetService;
            _leaveService = leaveService;
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
        public async Task<IActionResult> GetEmployeeProfile()
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _employeeServices.GetEmployeeById(empid);
            }
        }

        [HttpPost("Log-Working-Hours")]
        public async Task<IActionResult> LogWorkingHours([FromBody] LogWorkingHoursDTO logWorkingHoursDTO)
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _employeeServices.LogWorkingHours(logWorkingHoursDTO,empid);
            }
        }

        [HttpPatch("Update-Employee")]
        public async Task<IActionResult> UpdateEmployeeByEmployee([FromBody] JsonPatchDocument<UpdateEmployeeByEmployeeDTO> patchDoc )
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
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
        public async Task<IActionResult> GetTimeSheetById()
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _timeSheetService.GetTimeSheet(empid);
            }
        }
        
        [HttpPost("Update-TimeSheet")]
        public async Task<IActionResult> UpdateTimeSheet([FromBody]TimeSheetUpdateDTO timeSheetupdate)
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await _timeSheetService.UpdateTimeSheet(timeSheetupdate,empid);
            }
        }

        [HttpGet("Get-leaves")]
        public async Task<IActionResult> GetLeavesByEmployeeId()
        {
            var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
            if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
            {
                return BadRequest("Invalid or missing EmployeeId claim.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                
                return await _leaveService.GetLeavesByEmployeeIdAsync(empid);
            }
        }              

        [HttpPost("Apply-for-leave")]
        public async Task<IActionResult> AddLeave([FromBody] LeaveDTO leaveDTO)
        {
            try
            {
                var empIdClaim = User.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;
                if (string.IsNullOrEmpty(empIdClaim) || !int.TryParse(empIdClaim, out int empid))
                {
                    return BadRequest("Invalid or missing EmployeeId claim.");
                }
                bool alreadyApplied = await _leaveService.HasLeaveOnDateAsync(empid, leaveDTO.StartDate, leaveDTO.EndDate);

                if (alreadyApplied)
                {
                    return Conflict(new
                    {
                        Message = "You have already applied for leave on this date."
                    });
                }

                await _leaveService.AddLeaveAsync(leaveDTO, empid);
                return CreatedAtAction(nameof(GetLeaveById), new { id = empid }, leaveDTO);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveById(int id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null) return NotFound();
            return Ok(leave);
        }
    }
}

