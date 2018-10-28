using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm.Models
{
    /// <summary>
    /// Class that holds the ilp specific data belonging to a route calculation
    /// </summary>
    public class ILPStarterData
    {
        // General
        public int TimeSliceDuration { get; set; }

        // Phase 1
        public double ClusteringMIPGap { get; set; }
        public long ClusteringTimeLimit { get; set; }

        // Phase 2
        public double SchedulingMIPGap { get; set; }
        public long SchedulingTimeLimit { get; set; }
    }
}
