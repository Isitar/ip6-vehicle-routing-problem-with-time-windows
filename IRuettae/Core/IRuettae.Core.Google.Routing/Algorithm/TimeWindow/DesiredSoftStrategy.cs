using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm.TimeWindow
{
    /// <summary>
    /// use soft constraint for desired
    /// </summary>
    internal class DesiredSoftStrategy : ITimeWindowStrategy
    {
        public void AddConstraints(RoutingData data, RoutingModel model, IntVar cumulTime, RoutingDimension timeDim, int visit)
        {
            // forbid visit in unavailable
            new UnavailableOnlyStrategy().AddConstraints(data, model, cumulTime, timeDim, visit);

            // add soft time window for desired
            var desired = InternalSolver.GetDesired(data, visit);
            if (desired.HasValue)
            {
                var cost = InternalSolver.GetDesiredCoefficient(data, visit);
                timeDim.SetCumulVarSoftUpperBound(visit, desired.Value.to, cost);
                timeDim.SetCumulVarSoftLowerBound(visit, desired.Value.from, cost);
            }
        }
    }
}
