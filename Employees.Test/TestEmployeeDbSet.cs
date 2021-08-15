using System.Linq;
using Employees.Models;

namespace Employees.Test
{
    class TestEmployeeDbSet : TestDbSet<Employee>
    {
        public override Employee Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (int)keyValues.Single());
        }
    }
}