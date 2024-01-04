using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Helpers
{
    public static class StringHelper
    {
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        public static string ToFormattedDecimal(this decimal value)
        {
            return value.ToString("0.00");
        }
    }
}
