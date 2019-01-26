using Microsoft.AspNetCore.Mvc;
using timesheet.business;

namespace timesheet.api.controllers
{
    [Route("api/v1/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var items = this.employeeService.GetEmployees();
            return new ObjectResult(items);
        }

        [HttpGet("getalltasks")]
        public IActionResult GetAllTasks()
        {
            var items = this.employeeService.GetAllTasks();
            return new ObjectResult(items);
        }
    }
}