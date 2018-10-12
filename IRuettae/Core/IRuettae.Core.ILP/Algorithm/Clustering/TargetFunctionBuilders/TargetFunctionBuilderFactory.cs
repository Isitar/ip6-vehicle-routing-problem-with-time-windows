using System;
using IRuettae.Core.ILP.Algorithm.Persistence;

namespace IRuettae.Core.ILP.Algorithm.Clustering.TargetFunctionBuilders
{
    internal static class TargetFunctionBuilderFactory
    {
        public static AbstractTargetFunctionBuilder Create(ClusteringOptimizationGoals goal)
        {
            switch (goal)
            {
                case ClusteringOptimizationGoals.MinTimePerSanta:
                    return new MinTimePerSantaTargetFunctionBuilder();
                case ClusteringOptimizationGoals.OverallMinTime:
                    return new OverallMinTimeTargetFunctionBuilder();
                case ClusteringOptimizationGoals.MinAvgTimePerSanta:
                    return new MinAvgTimeTargetFunctionBuilder();
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
