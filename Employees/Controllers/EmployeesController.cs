using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employees.DataAccess;
using Employees.Models;
using Employees.Validators;
using FluentValidation.Results;

namespace Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int? bossId = null, string firstName = null, DateTime? birthDate1 = null, DateTime? birthDate2 = null)
        {
            return await _context.Employee
                .Where(e => bossId.HasValue ? e.Boss == bossId : true &&
                    !string.IsNullOrWhiteSpace(firstName) ? e.FirstName.Contains(firstName) : true &&
                    (birthDate1.HasValue && birthDate2.HasValue) ? (e.BirthDate > birthDate1 && e.BirthDate < birthDate2) : true).ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("employeeId/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // GET: api/Employees/5
        [HttpGet("role/{role}")]
        public async Task<ActionResult<object>> GetEmployeeCountAndAverageSalaryByRole(string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role))
                {
                    return BadRequest();
                }

                var query = await (from employee in _context.Employee
                                   where employee.Role == role
                                   group employee by 1 into grp
                                   select new
                                   {
                                       employeeCount = grp.Count(),
                                       averageSalary = Convert.ToInt32(grp.Sum(e => e.CurrentSalary)) / grp.Count()
                                   }).FirstAsync();

                if (query == null)
                {
                    return NotFound();
                }

                return query;
            }
            catch (DivideByZeroException ex)
            {
                log.Error("Dividing by zero is not allowed", ex);
                return BadRequest();
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            try
            {
                EmployeeValidator validator = new EmployeeValidator();
                ValidationResult result = validator.Validate(employee);

                if (result.IsValid)
                {
                    _context.Entry(employee).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(result.ToString(Environment.NewLine));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Employees/6
        [HttpPut("employeeId/{id}")]
        // salary det i body, kad neexposint parametro.
        public async Task<IActionResult> PutEmployeeSalary(int id, Employee employee)
        {
            if (employee.CurrentSalary < 0)
            {
                return BadRequest();
            }

            var employeeInDb = await _context.Employee.FindAsync(id);

            if (employeeInDb == null)
            {
                return NotFound();
            }

            employeeInDb.CurrentSalary = employee.CurrentSalary;

            EmployeeValidator validator = new EmployeeValidator();
            ValidationResult result = validator.Validate(employeeInDb);

            try
            {
                if (result.IsValid)
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(result.ToString(Environment.NewLine));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                EmployeeValidator validator = new EmployeeValidator();
                ValidationResult result = validator.Validate(employee);

                if (result.IsValid)
                {
                    _context.Employee.Add(employee);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(result.ToString(Environment.NewLine));
                }

            }
            catch (DbUpdateException ex)
            {
                log.Error("Employee rule violation", ex);
                return BadRequest("Employee rule violation: there can only be one CEO");
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
