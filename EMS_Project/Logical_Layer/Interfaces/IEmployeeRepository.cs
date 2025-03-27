using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<object> GetEmployeeAsync();
        public  Task<object?> GetAdminEmployeeByEmailAsync(string email);

        public Task<object?> GetEmployeeByIdAsync(int id);
        public Task<Employee?> CheckIfEmployeeExistsByEmail(string email);
        public Task<Employee?> CheckIfEmployeeExistsById(int id);
        public Task<Department?> CheckIfDepartmentExists(int id);
        public  Task<object?> AddEmployee(Employee employee);
        public Task DeleteEmployee(string email);
        public Task<object?> UpdateEmployee(Employee employee);
        public Task<Timesheet?> logWorkingHours(Timesheet timesheet);
    }
}
