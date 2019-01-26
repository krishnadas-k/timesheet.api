using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace timesheet.model
{
    public class Effort
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double EffortInHrs { get; set; }

        public string Remarks { get; set; }

        public Employee Employee { get; set; }

        public Task Task { get; set; }
    }
}
