using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestExamples.Calc.Tests.Extensions
{
    public static class DateTimeListExtensions
    {
        public static bool IsDailyTimeSeries(this IList<DateTime> dates)
        {
            return IsTimeSeries(dates, x => x.AddDays(1));
        }

        public static bool IsMonthlyTimeSeries(this IList<DateTime> dates)
        {
            return IsTimeSeries(dates, x => x.AddMonths(1));
        }

        private static bool IsTimeSeries(IList<DateTime> dates, Func<DateTime, DateTime> getNextDate)
        {
            var previousDate = dates.First();
            foreach (var date in dates.Skip(1))
            {
                if (getNextDate(previousDate) != date)
                {
                    return false;
                }

                previousDate = date;
            }

            return true;
        }
    }
}
