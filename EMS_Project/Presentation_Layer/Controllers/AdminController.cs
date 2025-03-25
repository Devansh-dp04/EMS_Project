
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Logical_Layer.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Presentation_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmployeeServices _employeeServices;
    private readonly ITimeSheetService _timeSheetService;

    public AdminController(IAuthService authService, IEmployeeServices employeeServices, ITimeSheetService timeSheetService)
    {
        _authService = authService;
        _employeeServices = employeeServices;
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

    [HttpGet("get-employee")]
    public async Task<IActionResult> GetEmployee()
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _employeeServices.GetEmployee();
        }
    }

    [HttpGet("get-employee-By-Id")]
    public async Task<IActionResult> GetEmployeeById([FromQuery] int id)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _employeeServices.GetEmployeeById(id);
        }
    }

    [HttpGet("Export-TimeSheet-To-Excel")]
    public async Task<IActionResult> ExportTimeSheet()
    {
        var filedata = await _timeSheetService.ExportToExcelAsync();

        if (filedata == null)
        {
            return NotFound("No Data to Export");
        }
        var fileName = $"Timesheet_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        return File(filedata, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpPost("add-employee")]
    public async Task<IActionResult> AddEmployee(AddEmployeeDTO addEmployee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _employeeServices.AddEmployee(addEmployee);
        }
    }

    [HttpPost("delete-employee")]
    public async Task<IActionResult> DeleteEmployee(string email)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _employeeServices.DeleteEmployee(email);
        }
    }

    [HttpPatch("Update-Employee")]
    public async Task<IActionResult> UpdateEmployeeByAdmin([FromBody] JsonPatchDocument<UpdateEmployeeByAdminDTO> patchDoc, [FromQuery] int empid)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _employeeServices.UpdateEmployeeByAdmin(patchDoc, empid);
        }
    }

    [HttpPost("Reset-Password")]
    public async Task<IActionResult> AdminResetPassword(string email)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            return await _authService.ResetTokenGeneration(email, "admin");
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

    

}
