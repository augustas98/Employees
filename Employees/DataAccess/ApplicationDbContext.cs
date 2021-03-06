using Microsoft.EntityFrameworkCore;
using Employees.Models;

namespace Employees.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        { }

        public DbSet<Employee> Employee { get; set; }

        public void MarkAsModified(Employee employee)
        {
            Entry(employee).State = EntityState.Modified;
        }
    }
}
