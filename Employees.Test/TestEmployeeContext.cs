using System;
using System.Data.Entity;
using Employees.Models;

namespace Employees.Test
{
    public class TestEmployeeContext : IEmployeeContext
    {
        public TestEmployeeContext()
        {
            this.Employees = new TestEmployeeDbSet();
        }

        public DbSet<Employee> Employees { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Employee employee) { }
        public void Dispose() { }
    }
}