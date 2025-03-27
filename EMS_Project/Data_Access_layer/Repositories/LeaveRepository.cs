using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Data_Access_layer.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {   
        private readonly EMSDbContext _context;
        public LeaveRepository(EMSDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Leave>> GetAllLeavesAsync()
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .ToListAsync();
        }
        public async Task<IEnumerable<Leave>> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }
        public async Task<Leave?> GetLeaveByIdAsync(int leaveid)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.LeaveId == leaveid);
        }

        public async Task AddLeaveAsync(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasLeaveOnDateAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Leaves
                .AnyAsync(l => l.EmployeeId == employeeId &&
                               ((l.StartDate <= endDate && l.EndDate >= startDate)));
        }
        public async Task<bool> UpdateLeaveStatusAsync(string leavestatus, int empid)
        {
            
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.EmployeeId == empid);

            if (leave == null)
            {
                return false;  
            }
            
            leave.Status = leavestatus;
           
            _context.Leaves.Update(leave);
            
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }


}
