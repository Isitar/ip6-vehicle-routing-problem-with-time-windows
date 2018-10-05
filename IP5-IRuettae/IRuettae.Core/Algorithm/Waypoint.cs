using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    public struct Waypoint
    {
        public int Visit;

        /// <summary>
        /// in timeslice
        /// </summary>
        public int StartTime;

        public long RealVisitId;

        public Waypoint(int visit, int startTime, long realVisitId)
        {
            Visit = visit;
            StartTime = startTime;
            RealVisitId = realVisitId;
        }

        public Waypoint(int visit, int startTime)
        {
            Visit = visit;
            StartTime = startTime;
            RealVisitId = visit;
        }
        public override string ToString()
        {
            return $"{Visit} | {StartTime}";
        }
    }
}
