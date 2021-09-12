namespace Employees.Models
{
    public class EmployeeCountAndSalary
    {
        public int Count { get; set; }

        public long Salary { get; set; }

        public override bool Equals(object obj)
        {
            var countAndSalary = obj as EmployeeCountAndSalary;

            if (countAndSalary == null)
            {
                return false;
            }

            return (Count == countAndSalary.Count)
                && (Salary == countAndSalary.Salary);
        }

        public override int GetHashCode()
        {
            return Count.GetHashCode() ^ Salary.GetHashCode();
        }

    }
}
