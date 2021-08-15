using Employees.Models;
using System.Data.Entity;

namespace Employees.Test
{
    public class TestEmployeeContext : IEmployeeContext
    {
        public TestEmployeeContext()
        {
            this.Employee = new TestEmployeeDbSet();
        }

        public DbSet<Employee> Employee { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Employee employee) { }
        public void Dispose() { }
    }
}