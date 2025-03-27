using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EMS_Project.Logical_Layer.CustoDataAnnotation;
using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class LogWorkingHoursDTO
    {
        //[Required]
        //public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ValidWorkingHours]
        public TimeSpan StartTime { get; set; }
        [ValidWorkingHours]
        [Required]
        public TimeSpan EndTime { get; set; }        

        [StringLength(500)]
        public string Description { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal TotalHours => (decimal)(EndTime - StartTime).TotalHours;

    }

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {            
            CreateMap<LogWorkingHoursDTO, Timesheet>()
                .ForMember(dest => dest.TimesheetId, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))  
                .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => (decimal)(src.EndTime - src.StartTime).TotalHours));
        }
    }
}
