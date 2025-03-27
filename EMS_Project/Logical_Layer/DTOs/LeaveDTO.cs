using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EMS_Project.Logical_Layer.CustoDataValidation;
using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class LeaveDTO
    {
        //[Required]
        //public int EmployeeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be greater than start date.")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string LeaveType { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; } = "Pending";  // Default status

    }
    public class LeaveMappingProfile : Profile
    {
        public LeaveMappingProfile()
        {
            CreateMap<LeaveDTO, Leave>();
        }
    }
}
