using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core
{
    public static class Utility
    {
        /// <summary>
        /// Returns how much the two intervals overlap
        /// </summary>
        /// <param name="start1">start of first interval</param>
        /// <param name="end1">end of first interval</param>
        /// <param name="start2">start of second interval</param>
        /// <param name="end2">end of second interval</param>
        /// <returns></returns>
        public static int IntersectionLength(int start1, int end1, int start2, int end2)
        {
            int startIntersection = Math.Max(start1, start2);
            int endIntersection = Math.Min(end1, end2);
            if (startIntersection < endIntersection)
            {
                return endIntersection - startIntersection;
            }
            return 0;
        }
    }
}
