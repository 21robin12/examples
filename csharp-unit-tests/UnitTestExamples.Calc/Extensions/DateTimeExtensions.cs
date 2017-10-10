using System;

namespace UnitTestExamples.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
