using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm.Persistence
{
    public enum ClusteringOptimizationGoals
    {
        OverallMinTime,
        MinTimePerSanta,
        MinAvgTimePerSanta, // Default
    }
}
