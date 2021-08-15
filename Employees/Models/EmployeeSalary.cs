using System.ComponentModel.DataAnnotations;

namespace Employees.Models
{
    public class EmployeeSalary
    {
        [Required(ErrorMessage = "CurrentSalary is required")]
        [Range(0, long.MaxValue, ErrorMessage = "CurrentSalary should be greater than or equal to 0")]
        public long CurrentSalary { get; set; }
    }
}
