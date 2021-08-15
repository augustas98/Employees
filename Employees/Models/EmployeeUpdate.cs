using System;
using System.ComponentModel.DataAnnotations;

namespace Employees.Models
{
    public class EmployeeUpdate
    {

        [StringLength(50, ErrorMessage = "Must be under 50 characters")]
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Must be under 50 characters")]
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "EmploymentDate is required")]
        public DateTime EmploymentDate { get; set; }

        public int? Boss { get; set; }

        [Required(ErrorMessage = "HomeAddress is required")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "CurrentSalary is required")]
        [Range(0, long.MaxValue, ErrorMessage = "CurrentSalary should be greater than or equal to 0")]
        public long CurrentSalary { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

    }
}
