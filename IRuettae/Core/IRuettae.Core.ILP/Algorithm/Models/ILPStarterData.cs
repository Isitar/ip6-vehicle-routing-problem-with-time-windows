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
        // Phase 1
        public ClusteringOptimizationGoals ClusteringOptimizationFunction { get; set; }
        public double ClusteringMipGap { get; set; }
        public long ClusteringTimeLimit { get; set; }
        public string ClusteringResult { get; set; }

        // Phase 2
        public int TimeSliceDuration { get; set; }
        public double SchedulingMipGap { get; set; }
        public long SchedulingTimeLimit { get; set; }
        public string SchedulingResult { get; set; }
    }
}
