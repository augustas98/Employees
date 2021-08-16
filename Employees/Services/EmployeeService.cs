using Employees.DataAccess;
using Employees.Models;
using Employees.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private EmployeeValidator _validator = new EmployeeValidator();

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> FindEmployee(int id)
        {
            return await _context.Employee.FindAsync(id);
        }

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int? bossId = null, string firstName = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null)
        {
            return await _context.Employee.Where(
                e => (bossId == null || e.Boss == bossId) &&
                (firstName == null || e.FirstName.Contains(firstName)) &&
                (birthDateFrom == null || e.BirthDate > birthDateFrom) &&
                (birthDateTo == null || e.BirthDate < birthDateTo)).ToListAsync();
        }

        public async Task<EmployeeCountAndSalary> GetEmployeeCountAndAverageSalaryByRole(string role)
        {
            EmployeeCountAndSalary countAndSalary = new EmployeeCountAndSalary { };

            var employeeByRole = await _context.Employee.Where(e => e.Role.Contains(role)).FirstOrDefaultAsync();

            if (employeeByRole == null)
            {
                return countAndSalary;
            }

            var query = await (from employee in _context.Employee
                               where employee.Role.Contains(role)
                               group employee by 1 into grp
                               select new
                               {
                                   employeeCount = grp.Count(),
                                   averageSalary = Convert.ToInt32(grp.Sum(e => e.CurrentSalary)) / grp.Count()
                               }).FirstAsync();

            countAndSalary.Count = query.employeeCount;
            countAndSalary.Salary = query.averageSalary;

            return countAndSalary;
        }

        public ValidationResult GetEmployeeValidationResult(Employee employee)
        {
            return _validator.Validate(employee);
        }

        public async Task<Employee> GetFirstEmployeeByRole(string role)
        {
            return await _context.Employee.Where(e => e.Role.Contains(role)).FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
        }

        public void EmployeeSetValues(Employee employeeInDb, EmployeeUpdate employee)
        {
            _context.Entry(employeeInDb).CurrentValues.SetValues(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            _context.Employee.Remove(employee);
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
