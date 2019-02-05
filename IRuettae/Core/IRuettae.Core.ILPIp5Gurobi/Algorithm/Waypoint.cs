using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILPIp5Gurobi.Algorithm
{
    public struct Waypoint
    {
        public int Visit;

        /// <summary>
        /// in timeslice
        /// </summary>
        public int StartTime;

        public Waypoint(int visit, int startTime)
        {
            Visit = visit;
            StartTime = startTime;
        }
        public override string ToString()
        {
            return $"{Visit} | {StartTime}";
        }
    }
}
