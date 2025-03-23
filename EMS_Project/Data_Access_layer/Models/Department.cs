using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Project.Models
{
    public class Department
    {
        [Key]        
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property for one-to-many relationship with Employee
        public virtual ICollection<Employee> Employees { get; set; }
    }
} 