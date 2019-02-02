using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm.TimeWindow
{
    /// <summary>
    /// use hard-constraint on desire
    /// hard-constraint on unavailable otherwise
    /// </summary>
    internal class DesiredHardStrategy : ITimeWindowStrategy
    {
        public void AddConstraints(RoutingData data, RoutingModel model, IntVar cumulTime, RoutingDimension timeDim, int visit)
        {
            var desired = InternalSolver.GetDesired(data, visit);
            if (desired.HasValue)
            {
                // add soft time window for desired
                cumulTime.SetRange(desired.Value.from, desired.Value.to);
            }
            else
            {
                // forbid visit in unavailable
                new UnavailableOnlyStrategy().AddConstraints(data, model, cumulTime, timeDim, visit);
            }
        }
    }
}
