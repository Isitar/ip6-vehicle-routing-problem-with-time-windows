using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public static class Utility
    {
        /// <summary>
        /// Returns the length of the given desired,
        /// which overlaps with the working hours.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="desired"></param>
        /// <returns></returns>
        public static int GetRealDesiredLength(RoutingData data, (int from, int to) desired)
        {
            return data.Input.Days.Select(day => Core.Utility.IntersectionLength(desired.from, desired.to, day.from, day.to)).Append(0).Max();
        }
    }
}
