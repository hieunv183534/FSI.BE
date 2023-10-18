using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Lib
{
    public static class Utils
    {
        public static uint ToDoUInt32DateTime(this DateTime dateTime)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan currTime = dateTime - startTime;
            uint time_t = Convert.ToUInt32(Math.Abs(currTime.TotalSeconds));
            return time_t;
        }
    }
}
