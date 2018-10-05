using System;

namespace IRuettae.Core.ILP.Algorithm.Clustering.TargetFunctionBuilders
{
    internal static class TargetFunctionBuilderFactory
    {
        public static AbstractTargetFunctionBuilder Create(TargetBuilderType type)
        {
            switch (type)
            {
                case TargetBuilderType.Default:
                    return new MinTimePerSantaTargetFunctionBuilder();
                case TargetBuilderType.MinTimeOnly:
                    return new OverallMinTimeTargetFunctionBuilder();
                case TargetBuilderType.MinAvgTimeOnly:
                    return new MinAvgTimeTargetFunctionBuilder();
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
