using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.Interfaces
{  
        public interface IAdminRepository
        {
            public Task<Admin> GetAdminByEmailAsync(string email);
            public Task<Admin?> CheckAdminExistByEmailAsync(string email);
            public Task<Admin?> UpdateAdmin(Admin admin);   
       
    }
    
}
