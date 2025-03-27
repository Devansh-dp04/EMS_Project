using EMS_Project.Logical_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface ILeaveService
    {
        
        public Task<LeaveDTO> GetLeaveByIdAsync(int id);
        public Task<IActionResult> GetLeavesByEmployeeIdAsync(int employeeId);
        public Task AddLeaveAsync(LeaveDTO leaveDTO, int empid);
        public Task<bool> HasLeaveOnDateAsync(int employeeId, DateTime startDate, DateTime endDate);
        public Task<IActionResult> UpdateLeaveStatusAsync(int empid, string leavestatus);
        public Task<bool> DeleteLeaveAsync(int id);
    }
}
