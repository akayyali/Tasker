using System;

namespace Tasker
{
    public static class Extensions
    {
        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;

            return start.AddDays(daysToAdd) > DateTime.Now ?
                start.AddDays(daysToAdd) :
                start.AddDays(7);
        }
    }
}
