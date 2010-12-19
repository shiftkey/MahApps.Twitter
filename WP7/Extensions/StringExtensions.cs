using System;

namespace MahApps.Twitter.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ParseDateTime(this string date)
        {
            var month = date.Substring(4, 3).Trim();
            var dayInMonth = date.Substring(8, 2).Trim();
            var time = date.Substring(11, 9).Trim();
            var year = date.Substring(25, 5).Trim();
            var dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);

            DateTime output;
            DateTime.TryParse(dateTime, out output);

            return output;
        }
    }
}
