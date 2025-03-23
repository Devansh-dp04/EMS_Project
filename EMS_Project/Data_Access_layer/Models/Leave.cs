using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS_Project.Models
{
    public class Leave
    {
        [Key]
       
        public int LeaveId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        
        public DateTime StartDate { get; set; }

        [Required]
        
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string LeaveType { get; set; }

        public string Reason { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedAt { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
} 