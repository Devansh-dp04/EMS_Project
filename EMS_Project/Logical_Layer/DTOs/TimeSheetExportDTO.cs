﻿namespace EMS_Project.Logical_Layer.DTOs
{
    public class TimeSheetExportDTO
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
