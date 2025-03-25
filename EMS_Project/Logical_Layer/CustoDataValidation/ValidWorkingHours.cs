using System.ComponentModel.DataAnnotations;
using EMS_Project.Logical_Layer.DTOs;

namespace EMS_Project.Logical_Layer.CustoDataAnnotation
{
    public class ValidWorkingHoursAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dto = validationContext.ObjectInstance;
            // Use reflection to get StartTime and EndTime properties
            var startTimeProperty = dto.GetType().GetProperty("StartTime");
            var endTimeProperty = dto.GetType().GetProperty("EndTime");

            if (startTimeProperty == null || endTimeProperty == null)
            {
                return new ValidationResult("StartTime and EndTime properties are required.");
            }
            var startTime = (TimeSpan)startTimeProperty.GetValue(dto);
            var endTime = (TimeSpan)endTimeProperty.GetValue(dto);
            if (startTime >= endTime)
            {
                return new ValidationResult("Start time must be less than end time.");
            }

            var totalHours = (endTime - startTime).TotalHours;

            if (totalHours > 8.5)
            {
                
                endTime = startTime.Add(TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30)));
            }

            return ValidationResult.Success;
        }
    }
}
