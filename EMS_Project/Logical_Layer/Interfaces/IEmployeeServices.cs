using EMS_Project.Logical_Layer.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface IEmployeeServices
    {
        public  Task<IActionResult> GetEmployee();
        public Task<IActionResult> GetEmployeeById(int id);
        public Task<IActionResult> AddEmployee(AddEmployeeDTO addEmployee);

        public Task<IActionResult> DeleteEmployee(string email);

        public Task<IActionResult> UpdateEmployee(JsonPatchDocument<UpdateEmployeeDTO> patchDoc, int empid);

    }
}
