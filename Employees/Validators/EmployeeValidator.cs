using System;
using Employees.Models;
using FluentValidation;

namespace Employees.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public const string role = "CEO";

        public EmployeeValidator()
        {
            RuleFor(employee => employee.FirstName).Length(0, 50);

            RuleFor(employee => employee.LastName).Length(0, 50);

            RuleFor(employee => employee.FirstName).NotEqual(employee => employee.LastName)
                .WithMessage("First name and last name cannot be equal");

            RuleFor(employee => Employee.GetAgeByDateTime(employee.BirthDate))
                .GreaterThan(18).WithMessage("Employee's age must be greater than 18")
                .LessThan(70).WithMessage("Employee's age must be less than 70");

            RuleFor(employee => employee.EmploymentDate).GreaterThan(new DateTime(2000, 1, 1));

            RuleFor(employee => employee.EmploymentDate).LessThan(DateTime.Now);

            RuleFor(employee => employee.Boss).Null().When(employee => employee.Role == role);

            RuleFor(employee => employee.CurrentSalary).GreaterThanOrEqualTo(0).NotNull();
        }
    }
}
