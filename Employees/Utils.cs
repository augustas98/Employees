using System;

namespace Employees
{
    public static class Utils
    {
        public static int GetAgeByDateTime(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}
