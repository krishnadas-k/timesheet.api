using Microsoft.AspNetCore.Mvc;
using System;
using timesheet.business;
using timesheet.model.DTO;

namespace timesheet.api.controllers
{
    [Route("api/v1/timesheet")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly EmployeeService employeeService;
        public TimeSheetController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPost("gettimesheet")]
        public IActionResult GetTimeSheet(TimeSheetArg arg)
        {
            // Start date can come from the client.
            // By default it is given as 7 days in this week
            DateTime dayStart = arg.StartTime ?? DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));
            DateTime dayEnd = arg.EndTime ?? dayStart.AddDays(7);


            var timeSheet = this.employeeService.GetTimeSheet(dayStart, dayEnd, arg.EmployeeId);
            return new ObjectResult(timeSheet);
        }

        [HttpPost("save")]
        public IActionResult SaveTimeSheet(EmployeeTimeSheetDTO timeSheet)
        {
            var items = this.employeeService.SaveTimeSheet(timeSheet);
            return new ObjectResult(items);
        }
    }
}