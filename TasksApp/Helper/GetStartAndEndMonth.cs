using System;

namespace ConsoleApp1
{
    public static class GetStartAndEndMonth
    {
        private static readonly int[] DaysOfMonthsNonLeap =
        { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private static readonly int[] DaysOfMonthLeap =
        { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private static bool isLeapyear(int year)
        {
            if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                return true;
            else
                return false;
        }

        public static (int startDayOfMonth, int endDayOfMonth) StartAndEndDateOfMonth(int year, int month)
        {
            if (year < 1 || year > 9999)
                throw new ArgumentOutOfRangeException("year");

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException("month");

            int[] days = isLeapyear(year) == true
                            ? DaysOfMonthLeap
                            : DaysOfMonthsNonLeap;

            return (1, days[month]);
        }
    }
}