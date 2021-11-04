using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker
{
    public class StartOnNextDayOfWeek
    {
        private int hour;
        private int minute;
        private int second;

        public DayOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// The Hour in 24h format (0 thourgh 23)
        /// </summary>
        public int Hour
        {
            get => hour;
            set
            {
                if (value < 0 || value > 23)
                    throw new ArgumentOutOfRangeException(nameof(Hour), $"Invalid value '{value}', the Hour is in 24h format (0 thourgh 23)");
                hour = value;
            }
        }
        /// <summary>
        /// The Minute (0 thourgh 59)
        /// </summary>
        public int Minute
        {
            get => minute;
            set
            {
                if (value < 0 || value > 59)
                    throw new ArgumentOutOfRangeException(nameof(Minute), $"Invalid value '{value}', the Minute value should be netween (0 thourgh 59)");

                minute = value;
            }
        }
        /// <summary>
        /// The Second (0 thourgh 59)
        /// </summary>
        public int Second
        {
            get => second;
            set
            {
                if (value < 0 || value > 59)
                    throw new ArgumentOutOfRangeException(nameof(Second), $"Invalid value '{value}', the Second value should be netween (0 thourgh 59)");

                second = value;
            }
        }
    }

}
