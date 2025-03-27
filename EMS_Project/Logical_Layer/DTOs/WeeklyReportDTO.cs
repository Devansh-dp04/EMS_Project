namespace EMS_Project.Logical_Layer.DTOs
{
    public class WeeklyReportDTO
    {
        public int EmployeeId { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public decimal TotalHours { get; set; }
        public List<TimeSheetDTO> TimesheetEntries { get; set; }
    }
}
