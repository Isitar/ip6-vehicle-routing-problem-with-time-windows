using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm.TimeSlicing.TargetFunctionBuilders
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
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
