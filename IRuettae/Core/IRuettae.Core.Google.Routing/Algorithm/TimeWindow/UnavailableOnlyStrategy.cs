using System.Linq;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm.TimeWindow
{
    /// <summary>
    /// Only set hard constraint for unavailable.
    /// Ignore desired.
    /// </summary>
    internal class UnavailableOnlyStrategy : ITimeWindowStrategy
    {
        public void AddConstraints(RoutingData data, RoutingModel model, IntVar cumulTime, RoutingDimension timeDim, int visit)
        {
            // forbid visit in unavailable
            var unavailableStarts = data.Unavailable[visit].Select(u => u.startFrom).ToList();
            var unavailableEnds = data.Unavailable[visit].Select(u => u.startEnd).ToList();
            var constraint = model.solver().MakeNotMemberCt(cumulTime, new CpIntVector(unavailableStarts), new CpIntVector(unavailableEnds));
            model.solver().Add(constraint);
        }
    }
}
