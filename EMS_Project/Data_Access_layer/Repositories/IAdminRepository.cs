using EMS_Project.Models;

namespace EMS_Project.Data_Access_layer.Repositories
{  
        public interface IAdminRepository
        {
            public Task<Admin> GetAdminByEmailAsync(string email);
            //Task<Admin> GetAdminById(int id);
            //Task<Admin> AddAdmin(Admin admin);
            //Task<Admin> UpdateAdmin(Admin admin);
            //Task<Admin> DeleteAdmin(int id);
        }
    
}
