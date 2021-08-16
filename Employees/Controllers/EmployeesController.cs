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
        public const string ceoRole = "CEO";

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
        public async Task<ActionResult<EmployeeCountAndSalary>> GetEmployeeCountAndAverageSalaryByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest();
            }

             return await _employeeService.GetEmployeeCountAndAverageSalaryByRole(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeUpdate employee)
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
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            var firstExistingEmployeeByRole = await _employeeService.GetFirstEmployeeByRole(ceoRole);

            if (firstExistingEmployeeByRole != null && employee.Role == ceoRole)
            {
                return Conflict("Employee rule violation: there can only be one CEO");
            }

            ValidationResult result = _employeeService.GetEmployeeValidationResult(employee);

            if (!result.IsValid)
            {
                return BadRequest(result.ToString(Environment.NewLine));
            }

            employee.Id = 0;
            _employeeService.AddEmployee(employee);
            await _employeeService.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeById", new { id = employee.Id }, employee);
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
