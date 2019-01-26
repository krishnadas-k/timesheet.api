using System;
using System.Collections.Generic;
using System.Linq;

namespace timesheet.model.DTO
{
    public class TimeSheetDTO
    {
        public TimeSheetDTO()
        {
            EffortLogs = new List<EffortLogDTO>();
        }

        public DateTime Date { get; set; }
        public IEnumerable<EffortLogDTO> EffortLogs { get; set; }
        public double TotalEffort
        {
            get
            {
                return EffortLogs.Sum(i => i.EffortInHrs);
            }
        }
    }
}
