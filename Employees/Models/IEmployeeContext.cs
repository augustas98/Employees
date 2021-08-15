using System;

namespace Employees.Models
{
    public class IEmployeeContext : IDisposable
    {
        DbSet<Employee> Employees { get; }
        int SaveChanges();
        void MarkAsModified(Product item);
    }
}
