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
    }
}
