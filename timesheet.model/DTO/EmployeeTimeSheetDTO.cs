using System.Collections.Generic;

namespace timesheet.model.DTO
{
    public class EmployeeTimeSheetDTO
    {
        public EmployeeTimeSheetDTO()
        {
            TimeSheets = new List<TimeSheetDTO>();
        }

        public int EmployeeId { get; set; }
        public List<TimeSheetDTO> TimeSheets { get; set; }
    }
}
