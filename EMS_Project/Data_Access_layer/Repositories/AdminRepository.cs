using EMS_Project.Data_Access_layer.DbContext;
using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Data_Access_layer.Repositories
{
    public class AdminRepository :  IAdminRepository
    {
        private readonly EMSDbContext _context;

        public AdminRepository(EMSDbContext context)
        {
            _context = context;
        }
        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            return admin;
        }
    }
}
