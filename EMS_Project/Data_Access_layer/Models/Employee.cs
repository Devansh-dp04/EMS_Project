using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Project.Models
{
    public class Employee
    {
        [Key]        
        public int EmployeeId { get; set; }

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

        public string PasswordHash { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }
       
        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public string TechStack { get; set; }
        public Boolean IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
    }
} 