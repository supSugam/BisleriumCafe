
using BisleriumCafe.Model;

namespace BisleriumCafe.Helpers
{
    public class MonthWithIndex
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }
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

        public static List<MonthWithIndex> GetListOfMonthsWithIndex()
        {
            List<MonthWithIndex> months = [];

            for (int i = 1; i <= 12; i++)
            {

                months.Add(new MonthWithIndex
                {
                    Index = i,
                    Name = new DateTime(2021, i, 1).ToString("MMMM")
                });
            }

            return months;
        }


        public static bool IsFutureMonth(DateTime? date)
        {
            return date is not null && date.Value.Year >= DateTime.Now.Year && date.Value.Month > DateTime.Now.Month;
        }

        public static bool IsFirstDateAfterSecondDate(DateTime? firstDate, DateTime secondDate)
        {
            if(firstDate == null)
            {
                return false;
            }
            return firstDate.Value.Year > secondDate.Year || firstDate.Value.Month > secondDate.Month || firstDate.Value.Day > secondDate.Day;
        }

        public static DateTime GetStartOfDate(DateSpan dateSpan, DateTime date)
        {
            switch (dateSpan)
            {
                case DateSpan.Day:
                    return date.Date;
                case DateSpan.Week:
                    return date.Date.AddDays(-(int)date.DayOfWeek);
                case DateSpan.Month:
                    return new DateTime(date.Year, date.Month, 1);
                case DateSpan.Year:
                    return new DateTime(date.Year, 1, 1);
                default:
                    return date.Date;
            }
        }

        public static DateTime GetEndOfDate(DateSpan dateSpan, DateTime date)
        {
            switch (dateSpan)
            {
                case DateSpan.Day:
                    return date.Date.AddDays(1).AddTicks(-1);
                case DateSpan.Week:
                    return date.Date.AddDays(7 - (int)date.DayOfWeek).AddTicks(-1);
                case DateSpan.Month:
                    return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).AddTicks(-1);
                case DateSpan.Year:
                    return new DateTime(date.Year, 12, 31).AddTicks(-1);
                default:
                    return date.Date.AddDays(1).AddTicks(-1);
            }
        }
    }
}
