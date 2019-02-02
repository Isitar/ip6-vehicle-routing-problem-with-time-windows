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
    /// ignore desired and unavailable
    /// </summary>
    internal class NoneStrategy : ITimeWindowStrategy
    {
        public void AddConstraints(RoutingData data, RoutingModel model, IntVar cumulTime, RoutingDimension timeDim, int visit)
        {
            // add nothing
        }
    }
}
