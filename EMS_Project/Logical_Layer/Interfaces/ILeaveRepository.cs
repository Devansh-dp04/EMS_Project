using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface ILeaveRepository
    {
       public Task<IEnumerable<Leave>> GetAllLeavesAsync();
       public Task<Leave?> GetLeaveByIdAsync(int id);
        public Task<IEnumerable<Leave>> GetLeavesByEmployeeIdAsync(int employeeId);
        public Task AddLeaveAsync(Leave leave);
        public Task<bool> HasLeaveOnDateAsync(int employeeId, DateTime startDate, DateTime endDate);
        public Task<bool> UpdateLeaveStatusAsync(string leavestatus, int empid);
        public Task<bool> DeleteLeaveAsync(int id);
    }
}
