using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employees.Models;
using FluentValidation.Results;
using Employees.Services;

namespace Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEmployeeService _employeeService;
        private const int ceoRole = 1;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int? bossId = null, string firstName = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null)
        {
            return await _employeeService.GetEmployees(bossId, firstName, birthDateFrom, birthDateTo);
        }

        [HttpGet("employeeId/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.FindEmployee(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<EmployeeCountAndSalary>> GetEmployeeCountAndAverageSalaryByRole(int role)
        {
             return await _employeeService.GetEmployeeCountAndAverageSalaryByRole(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeUpdatePost employee)
        {
            var employeeInDb = await _employeeService.FindEmployee(id);

            if (employeeInDb == null)
            {
                return NotFound();
            }

            try
            {
                _employeeService.EmployeeSetValues(employeeInDb, employee);

                ValidationResult result = _employeeService.GetEmployeeValidationResult(employeeInDb);

                if (!result.IsValid)
                {
                    return BadRequest(result.ToString(Environment.NewLine));
                }

                await _employeeService.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_employeeService.EmployeeExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPut("{id}/salary")]
        public async Task<IActionResult> PutEmployeeSalary(int id, EmployeeSalary salary)
        {
            if (salary.CurrentSalary < 0)
            {
                return BadRequest();
            }

            var employeeInDb = await _employeeService.FindEmployee(id);

            if (employeeInDb == null)
            {
                return NotFound();
            }

            employeeInDb.CurrentSalary = salary.CurrentSalary;

            ValidationResult result = _employeeService.GetEmployeeValidationResult(employeeInDb);

            try
            {
                if (!result.IsValid)
                {
                    return BadRequest(result.ToString(Environment.NewLine));
                }

                await _employeeService.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_employeeService.EmployeeExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeUpdatePost employee)
        {
            var firstExistingEmployeeByRole = await _employeeService.GetFirstEmployeeByRole(employee.Role);

            if (firstExistingEmployeeByRole != null && employee.Role.Equals(ceoRole))
            {
                return Conflict("Employee rule violation: there can only be one CEO");
            }

            var newEmployee = _employeeService.CreateNewEmployee(employee);
            ValidationResult result = _employeeService.GetEmployeeValidationResult(newEmployee);

            if (!result.IsValid)
            {
                return BadRequest(result.ToString(Environment.NewLine));
            }

            _employeeService.AddEmployee(newEmployee);
            await _employeeService.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeById", new { id = newEmployee.Id }, newEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.FindEmployee(id);

            if (employee == null)
            {
                return NotFound();
            }

            _employeeService.RemoveEmployee(employee);
            await _employeeService.SaveChangesAsync();

            return NoContent();
        }
    }
}
