using System.Security.Cryptography;
using System.Text;
using EMS_Project.Logical_Layer.Interfaces;

namespace EMS_Project.Logical_Layer.Services
{
    public class PasswordGeneration : IPasswordService
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
