using System;

namespace IRuettae.Core.Algorithm.Clustering.TargetFunctionBuilders
{
    internal static class TargetFunctionBuilderFactory
    {
        public static AbstractTargetFunctionBuilder Create(TargetBuilderType type)
        {
            switch (type)
            {
                case TargetBuilderType.Default:
                    return new DefaultTargetFunctionBuilder();
                case TargetBuilderType.MinTimeOnly:
                    return new MinTimeOnlyTargetFunctionBuilder();
                case TargetBuilderType.MinAvgTimeOnly:
                    return new MinAvgTimeTargetFunctionBuilder();
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
