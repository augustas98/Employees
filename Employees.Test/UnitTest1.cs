using Xunit;
using Employees.Controllers;
using Employees.Models;
using System.Collections.Generic;
using System;
using Employees.DataAccess;

namespace Employees.Test
{
    public class UnitTest1
    {
        private readonly ApplicationDbContext _context;

        [Fact]
        public void GetAllEmployees_ShouldReturnAllEmployees()
        {
            var testEmployees = GetTestEmployees();
            var controller = new EmployeesController(_context);
        }

        private List<Employee> GetTestEmployees()
        {
            var testEmployees = new List<Employee>();
            testEmployees.Add(new Employee { Id = 1, FirstName = "Augustas", LastName = "Matorka", BirthDate = new DateTime(1970, 1, 1), Boss = null, CurrentSalary = 84198, EmploymentDate = new DateTime(2005, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 2, FirstName = "String1", LastName = "String11", BirthDate = new DateTime(1971, 1, 1), Boss = null, CurrentSalary = 8419, EmploymentDate = new DateTime(2001, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 3, FirstName = "String2", LastName = "String22", BirthDate = new DateTime(1972, 1, 1), Boss = null, CurrentSalary = 841, EmploymentDate = new DateTime(1970, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 4, FirstName = "String3", LastName = "String33", BirthDate = new DateTime(1973, 1, 1), Boss = null, CurrentSalary = 84, EmploymentDate = new DateTime(2016, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 5, FirstName = "String4", LastName = "String44", BirthDate = new DateTime(1974, 1, 1), Boss = null, CurrentSalary = 8, EmploymentDate = new DateTime(1970, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 6, FirstName = "String5", LastName = "String55", BirthDate = new DateTime(1975, 1, 1), Boss = null, CurrentSalary = 8419, EmploymentDate = new DateTime(5445, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });

            return testEmployees;
        }
    }
}
