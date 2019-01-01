using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal enum TargetType
    {
        MinTime, // the overall time should be minimised
        TryVisitEarly, // the visits should be made as early as possible
        TryVisitDesired, // try to visit on timeslices where the visit is desired
    }
}
