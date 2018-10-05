using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
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
                case TargetBuilderType.TryDesiredOnly:
                    return new TryDesiredOnlyTargetFunctionBuilder();
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
