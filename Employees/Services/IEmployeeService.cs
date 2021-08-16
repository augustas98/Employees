using Employees.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Employees.Services
{
    public interface IEmployeeService
    {
        Task<Employee> FindEmployee(int id);
        Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int? bossId = null, string firstName = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null);
        Task<EmployeeCountAndSalary> GetEmployeeCountAndAverageSalaryByRole(string role);
        ValidationResult GetEmployeeValidationResult(Employee employee);
        Task<Employee> GetFirstEmployeeByRole(string role);
        Task SaveChangesAsync();
        void AddEmployee(Employee employee);
        void EmployeeSetValues(Employee employeeInDb, EmployeeUpdate employee);
        void RemoveEmployee(Employee employee);
        public bool EmployeeExists(int id);
    }
}
