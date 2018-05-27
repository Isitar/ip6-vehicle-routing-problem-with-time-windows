using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm.Scheduling.TargetFunctionBuilders
{
    internal enum TargetType
    {
        MinTime, // the overall time should be minimised
        MinSantas, // the number of santas needed overall should be minimised
        MinSantaShifts, // the number of santas needed each day should be minimised
        TryVisitEarly, // the visits should be made as early as possible
        TryVisitDesired, // try to visit on timeslices where the visit is desired
    }
}
