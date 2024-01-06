using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Helpers
{
    public static class Date
    {
        public static string GetFormattedDate(this DateTime date)
        {
            return date.ToString("MMM dd, yyyy");
        }
        public static string ToFullFormattedDate(this DateTime date)
        {
            return date.ToString("MMM dd, yyyy hh:mm tt");
        }

        public static List<DateTime> GetWeekdaysList(DateTime endDate, int days)
        {
            List<DateTime> weekdaysList = new();

            for (int i = 0; i <= days; i++)
            {
                DateTime currentDate = endDate.AddDays(-i);

                // Exclude Saturdays and Sundays
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    weekdaysList.Add(currentDate);
                }
            }

            return weekdaysList;
        }

        public static bool AreSameDay(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }

        public static List<(int,string)> GetListOfMonthsWithIndex()
        {
            List<(int, string)> months = new();

            for (int i = 1; i <= 12; i++)
            {
                months.Add((i, new DateTime(2021, i, 1).ToString("MMMM")));
            }

            return months;
        }
    }
}
