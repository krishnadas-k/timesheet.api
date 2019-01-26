using System;

namespace timesheet.model.DTO
{
    public class TimeSheetArg
    {
        public int EmployeeId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
