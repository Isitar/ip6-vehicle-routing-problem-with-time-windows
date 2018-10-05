using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm
{
    public enum TargetBuilderType
    {
        Default,
        MinTimeOnly, // only minimises time
        TryDesiredOnly, // only minimises try desired
        MinAvgTimeOnly, // only min avg time
    }
}
