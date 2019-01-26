namespace timesheet.model.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double TotalEffort { get; set; }
        public int EffortLoggedDays { get; set; }
        public double AvgEffort
        {
            get
            {
                if (EffortLoggedDays > 0)
                    return TotalEffort / EffortLoggedDays;

                return 0;
            }
        }
    }
}
