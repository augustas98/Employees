using Employees.Controllers;
using Employees.Models;
using Employees.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Employees.Tests
{
    public class EmployeeTest
    {
        public Mock<IEmployeeService> mock = new Mock<IEmployeeService>();

        [Fact]
        public async void GetEmployees()
        {
            mock.Setup(p => p.GetEmployees(null, null, null, null)).ReturnsAsync(GetTestEmployees());
            EmployeesController emp = new EmployeesController(mock.Object);
            ActionResult<IEnumerable<Employee>> result = await emp.GetEmployees(null, null, null, null);
            Assert.Equal(GetTestEmployees(), result.Value);
        }

        [Fact]
        public async void FindEmployee()
        {
            mock.Setup(p => p.FindEmployee(1)).ReturnsAsync(GetEmployee());
            EmployeesController emp = new EmployeesController(mock.Object);
            ActionResult<Employee> result = await emp.GetEmployeeById(1);
            Assert.Equal(GetEmployee(), result.Value);
        }

        [Fact]
        public async void GetEmployeeCountAndAverageSalaryByRole()
        {
            mock.Setup(p => p.GetEmployeeCountAndAverageSalaryByRole("Employee")).ReturnsAsync(GetCountAndSalary());
            EmployeesController emp = new EmployeesController(mock.Object);
            ActionResult<EmployeeCountAndSalary> result = await emp.GetEmployeeCountAndAverageSalaryByRole("Employee");
            Assert.Equal(GetCountAndSalary(), result.Value);
        }

        private List<Employee> GetTestEmployees()
        {
            var testEmployees = new List<Employee>();
            testEmployees.Add(new Employee { Id = 1, FirstName = "Augustas", LastName = "Matorka", BirthDate = new DateTime(1970, 1, 1), Boss = null, CurrentSalary = 84198, EmploymentDate = new DateTime(2005, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });
            testEmployees.Add(new Employee { Id = 2, FirstName = "String1", LastName = "String11", BirthDate = new DateTime(1971, 1, 1), Boss = null, CurrentSalary = 8419, EmploymentDate = new DateTime(2001, 1, 1), HomeAddress = "string 12g. 51", Role = "Employee" });

            return testEmployees;
        }

        private Employee GetEmployee()
        {
            var testEmployee = new Employee 
            {   Id = 1,
                FirstName = "Augustas",
                LastName = "Matorka",
                BirthDate = new DateTime(1970, 1, 1),
                Boss = null, CurrentSalary = 84198,
                EmploymentDate = new DateTime(2005, 1, 1),
                HomeAddress = "string 12g. 51",
                Role = "Employee"
            };

            return testEmployee;
        }

        private EmployeeCountAndSalary GetCountAndSalary()
        {
            var countAndSalary = new EmployeeCountAndSalary
            { 
                Count = 15,
                Salary = 245355
            };

            return countAndSalary;
        }

    }
}
