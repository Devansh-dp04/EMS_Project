namespace EMS_Project.Logical_Layer.DTOs
{
    public class MonthlyReportDTO
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalHours { get; set; }
        public List<TimeSheetDTO> TimesheetEntries { get; set; }
    }

}
