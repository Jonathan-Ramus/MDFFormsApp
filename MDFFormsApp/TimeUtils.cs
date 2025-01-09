using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFFormsApp
{
    internal class TimeUtils
    {
        private static readonly double OffSet = DateTimeOffset.Now.TotalOffsetMinutes * 60;
        public static DateTime Parse(string timeString)
        {
            string pattern = "HH:mm:ss.fffffff";
            if (timeString.Length > pattern.Length)
            {
                timeString = timeString.Substring(0, pattern.Length);
            }
            else if (timeString.Length < pattern.Length)
            {
                if (timeString.Length == 8)
                {
                    timeString += ".";
                }
                timeString = timeString.PadRight(pattern.Length, '0');
            }
            return DateTime.ParseExact(timeString, pattern, CultureInfo.InvariantCulture).AddHours(OffSet);
        }
    }
}
