using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.ILP.Algorithm.Models;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal static class TargetFunctionBuilderFactory
    {
        public static ITargetFunctionBuilder Create(SchedulingOptimizationGoals type)
        {
            switch (type)
            {
                case SchedulingOptimizationGoals.Default:
                    return new DefaultTargetFunctionBuilder();
                case SchedulingOptimizationGoals.OldDefault:
                    return new MixedTargetFunctionBuilder();
                case SchedulingOptimizationGoals.MinTimeOnly:
                    return new MinTimeOnlyTargetFunctionBuilder();
                case SchedulingOptimizationGoals.TryDesiredOnly:
                    return new TryDesiredOnlyTargetFunctionBuilder();
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
