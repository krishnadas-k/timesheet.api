using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using timesheet.data;
using timesheet.model;
using timesheet.model.DTO;

namespace timesheet.business
{
    public class EmployeeService
    {
        #region Private Fields

        private TimesheetDb db;

        #endregion

        #region Constructor
        public EmployeeService(TimesheetDb dbContext)
        {
            db = dbContext;
        }
        #endregion

        #region Constants
        const int TotalDaysInWeek = 7;
        #endregion

        #region APIs

        /// <summary>
        /// Gets all the employess in the system, with the total and avg effort for this week
        /// </summary>
        /// <returns></returns>
        public IQueryable<EmployeeDTO> GetEmployees()
        {
            DateTime weekStart = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));
            DateTime weekEnd = weekStart.AddDays(TotalDaysInWeek);

            var employees = db.Employees.Include(i => i.Efforts).Select(e =>
                            new EmployeeDTO
                            {
                                Id = e.Id,
                                Code = e.Code,
                                Name = e.Name,
                                TotalEffort = e.Efforts.Where(f => f.Date >= weekStart && f.Date < weekEnd).Sum(i => i.EffortInHrs),
                                EffortLoggedDays = e.Efforts.Where(f => f.Date >= weekStart && f.Date < weekEnd).GroupBy(g => g.Date).Count()
                            });

            return employees;
        }

        /// <summary>
        /// Gets the master list of tasks available in the system
        /// </summary>
        /// <returns></returns>
        public IQueryable<Task> GetAllTasks()
        {
            return db.Tasks;
        }

        /// <summary>
        /// Gets the time sheet from a start date to an end date for the employee
        /// </summary>
        /// <returns></returns>
        public List<TimeSheetDTO> GetTimeSheet(DateTime startDate, DateTime endDate, int employeeId)
        {
            List<TimeSheetDTO> retList = new List<TimeSheetDTO>();
            var timeSheets = db.Efforts.Include(i => i.Task)
                            .Where(i => i.EmployeeId == employeeId && i.Date >= startDate && i.Date < endDate)
                            .GroupBy(i => i.Date)
                            .OrderBy(grp => grp.Key)
                            .Select(grp =>
                              new TimeSheetDTO
                              {
                                  Date = grp.Key,
                                  EffortLogs = grp.Select(j => new EffortLogDTO
                                  {
                                      EffortInHrs = j.EffortInHrs,
                                      TaskId = j.TaskId,
                                      TaskName = j.Task.Name
                                  })
                              }).ToList();

            DateTime iStartTime = startDate;

            // Fills the empty effort list for the days there is no effort logged
            while (iStartTime < endDate)
            {
                TimeSheetDTO sheet = timeSheets.FirstOrDefault(t => t.Date == iStartTime);
                if (sheet == null)
                {
                    sheet = new TimeSheetDTO() { Date = iStartTime };
                }

                retList.Add(sheet);
                iStartTime = iStartTime.AddDays(1);
            }

            return retList;
        }

        /// <summary>
        /// Save time sheet in to db
        /// </summary>
        public ResulDTO SaveTimeSheet(EmployeeTimeSheetDTO timeSheetDto)
        {
            ResulDTO result = new ResulDTO();
            try
            {
                foreach (var day in timeSheetDto.TimeSheets)
                {
                    foreach (var effort in day.EffortLogs)
                    {
                        Effort dbEffort = db.Efforts.FirstOrDefault(f => f.EmployeeId == timeSheetDto.EmployeeId && f.Date == day.Date
                                                    && f.TaskId == effort.TaskId);
                        if (dbEffort != null)
                        {
                            dbEffort.EffortInHrs = effort.EffortInHrs;
                            db.Efforts.Update(dbEffort);
                        }
                        else
                        {
                            dbEffort = new Effort
                            {
                                EmployeeId = timeSheetDto.EmployeeId,
                                Date = day.Date,
                                TaskId = effort.TaskId,
                                EffortInHrs = effort.EffortInHrs
                            };

                            db.Efforts.Add(dbEffort);
                        }
                    }
                }

                db.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                result.ErrorMsg = "Something went wrong !!, Please try again.";
            }

            return result;
        }

        #endregion
    }
}
