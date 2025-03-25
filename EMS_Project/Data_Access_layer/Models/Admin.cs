using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Project.Models
{
    public class Admin
    {
        [Key]        
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        public string PasswordHash { get; set; }  
        public DateTime CreatedAt { get; set; }         
        public DateTime UpdatedAt { get; set; } 
    }
} 