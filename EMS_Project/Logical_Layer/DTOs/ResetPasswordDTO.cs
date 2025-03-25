using System.ComponentModel.DataAnnotations;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }

        public string Token { get; set; }  

        public string NewPassword { get; set; }

        public string Role { get; set; }
    }
}
