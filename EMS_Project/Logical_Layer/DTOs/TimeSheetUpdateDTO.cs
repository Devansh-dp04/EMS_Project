using System.ComponentModel.DataAnnotations;
using EMS_Project.Logical_Layer.CustoDataAnnotation;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class TimeSheetUpdateDTO
    {
        
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [ValidWorkingHours]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public string Description { get; set; }

        public decimal TotalHours => (decimal)(EndTime - StartTime).TotalHours;
    }
}
